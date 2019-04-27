using HENG.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace HENG.Helpers
{
    public enum DownloadStartResult
    {
        Started,
        Error,
        AllreadyDownloaded,
    }

    public class BackgroundDownloadHelper
    {
        private BackgroundDownloader backgroundDownloader;

        public BackgroundDownloadHelper()
        {
            //BackgroundAccessStatus result = await BackgroundExecutionManager.RequestAccessAsync();

            BackgroundTransferGroup group = BackgroundTransferGroup.CreateGroup(Guid.NewGuid().ToString());
            group.TransferBehavior = BackgroundTransferBehavior.Serialized;

            backgroundDownloader = new BackgroundDownloader
            {
                TransferGroup = group
            };
        }

        public async Task<DownloadStartResult> Download(Uri sourceUri, CancellationTokenSource cts, Action<Exception> action)
        {
            var file = await CheckLocalFileExistsFromUriHashAsync(sourceUri);
            var downloadingAlready = await IsDownloading(sourceUri);
            if (file == null && !downloadingAlready)
            {
                await Task.Run(() =>
                {
                    var hash = SafeHashUri(sourceUri);
                    var task = StartDownloadAsync(sourceUri.OriginalString, cts, BackgroundTransferPriority.High, hash);
                    task.ContinueWith((state) =>
                    {
                        if (state.Exception != null)
                        {
                            action(new Exception($"An error occured with this download {state.Exception}"));
                        }
                        else
                        {
                            Trace.WriteLine("Download Completed");
                        }
                    });
                });
                return DownloadStartResult.AllreadyDownloaded;
            }
            else if (file != null)
            {
                return DownloadStartResult.AllreadyDownloaded;
            }
            else
            {
                return DownloadStartResult.Error;
            }
        }

        private string SafeHashUri(Uri sourceUri)
        {
            string safeUri = sourceUri.ToString().ToLower();
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(safeUri, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider hashAlgorithm = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            var hashByte = hashAlgorithm.HashData(buffer).ToArray();
            var sb = new StringBuilder(hashByte.Length * 2);
            foreach (byte b in hashByte)
            {
                sb.Append(b.ToString("x2"));
            }
            return $"{sb.ToString()}.jpg";
        }

        private async Task<IStorageItem> CheckLocalFileExistsFromUriHashAsync(Uri sourceUri)
        {
            string hash = SafeHashUri(sourceUri);
            StorageFolder folder = await AppSettingService.GetDeaultDownPathAsync();
            IStorageItem file = await folder.TryGetItemAsync(hash);
            return file;
        }

        private async Task<IStorageFile> GetLocalFileFromNameAsync(string hash)
        {
            StorageFile file = null;
            StorageFolder folder = await AppSettingService.GetDeaultDownPathAsync();
            try
            {
                file = await folder.CreateFileAsync(hash, CreationCollisionOption.ReplaceExisting);
            }
            catch (FileNotFoundException)
            {
            }
            return file;
        }

        private async Task StartDownloadAsync(string sourceUri, CancellationTokenSource cts, BackgroundTransferPriority priority, string hash)
        {
            var file = await GetLocalFileFromNameAsync(hash);
            var downloadOperation = backgroundDownloader.CreateDownload(new Uri(sourceUri), file);
            downloadOperation.Priority = priority;
            Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(obj =>
            {
                double val = (double)obj.Progress.BytesReceived / (double)obj.Progress.TotalBytesToReceive;
                Trace.WriteLine(val);
            });

            var downloadTask = downloadOperation.StartAsync().AsTask(cts.Token, progressCallback);

            try
            {
                await downloadTask;
                ResponseInformation response = downloadOperation.GetResponseInformation();
            }
            catch (Exception)
            {
                Trace.WriteLine("Download exception");
            }
        }

        private static async Task<bool> IsDownloading(Uri sourceUri)
        {
            var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            if (downloads.Where(dl => dl.RequestedUri == sourceUri).FirstOrDefault() != null)
            {
                return true;
            }
            return false;
        }
    }
}
