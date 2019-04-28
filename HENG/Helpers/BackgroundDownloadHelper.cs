using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace HENG.Helpers
{
    public enum DownloadStartResult
    {
        None,
        Started,
        Error,
        AllreadyDownloaded,
    }

    public static class BackgroundDownloadHelper
    {
        #region 共有方法
        public static async Task AttachToDownloadsAsync()
        {
            var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            foreach (var download in downloads)
            {
                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                await download.AttachAsync().AsTask(progressCallback);
            }
        }

        public static async Task<DownloadStartResult> Download(Uri sourceUri, Action<IStorageFile,Exception> action)
        {
            var hash = SafeHashUri(sourceUri);
            var file = await CheckLocalFileExistsFromUriHash(sourceUri);

            var downloadingAlready = await IsDownloading(sourceUri);

            if (file == null && !downloadingAlready)
            {
                await Task.Run(() =>
                {
                    var task = StartDownload(sourceUri, BackgroundTransferPriority.High, hash);
                    task.ContinueWith(async (state) =>
                    {
                        if (state.Exception != null)
                        {
                            action(null, state.Exception);
                        }
                        else
                        {
                            var cached = await CheckLocalFileExistsFromUriHash(sourceUri);
                            action(cached, null);
                            Debug.WriteLine("Download Completed");
                        }
                    });
                });
                return DownloadStartResult.Started;
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

        public static async Task<StorageFile> CheckLocalFileExistsFromUriHash(Uri sourceUri)
        {
            string hash = SafeHashUri(sourceUri);
            return await CheckLocalFileExists(hash);
        }

        #endregion

        private static async Task<bool> IsDownloading(Uri sourceUri)
        {
            var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            if (downloads.Where(dl => dl.RequestedUri == sourceUri).FirstOrDefault() != null)
            {
                return true;
            }

            return false;
        }

        private static string SafeHashUri(Uri sourceUri)
        {
            string Hash(string input)
            {
                IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
                HashAlgorithmProvider hashAlgorithm = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
                var hashByte = hashAlgorithm.HashData(buffer).ToArray();
                var sb = new StringBuilder(hashByte.Length * 2);
                foreach (byte b in hashByte)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }

            string safeUri = sourceUri.ToString().ToLower();
            var hash = Hash(safeUri);
            return $"{hash}.jpg";
        }

        private static void RegisterBackgroundTask(IBackgroundTrigger trigger)
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = "DownloadCompleteTrigger";
            builder.SetTrigger(trigger);

            BackgroundTaskRegistration task = builder.Register();
        }

        private static async Task<StorageFile> CheckLocalFileExists(string fileName)
        {
            StorageFile file = null;
            try
            {
                file = await ApplicationData.Current.LocalCacheFolder.GetFileAsync(fileName);
                var props = await file.GetBasicPropertiesAsync();
                if (props.Size == 0)
                {
                    await file.DeleteAsync();
                    return null;
                }
            }
            catch (FileNotFoundException)
            {
            }

            return file;
        }

        private static async Task StartDownload(Uri target, BackgroundTransferPriority priority, string localFilename)
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            StorageFile destinationFile;
            destinationFile = await GetLocalFileFromName(localFilename);

            var group = BackgroundTransferGroup.CreateGroup(Guid.NewGuid().ToString());
            group.TransferBehavior = BackgroundTransferBehavior.Serialized;

            BackgroundTransferCompletionGroup completionGroup = new BackgroundTransferCompletionGroup();

            // this will cause the app to be activated when the download completes and
            // CheckCompletionResult will be called for the final download state
            RegisterBackgroundTask(completionGroup.Trigger);

            BackgroundDownloader downloader = new BackgroundDownloader(completionGroup);
            downloader.TransferGroup = group;
            group.TransferBehavior = BackgroundTransferBehavior.Serialized;
            DownloadOperation download = downloader.CreateDownload(target, destinationFile);
            download.Priority = priority;

            completionGroup.Enable();

            Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
            var downloadTask = download.StartAsync().AsTask(progressCallback);

            try
            {
                await downloadTask;

                // Will occur after download completes
                ResponseInformation response = download.GetResponseInformation();
            }
            catch (Exception)
            {
                Debug.WriteLine("Download exception");
            }
        }

        private static void DownloadProgress(DownloadOperation obj)
        {
            Debug.WriteLine(obj.Progress.ToString());

            var progress = (double)obj.Progress.BytesReceived / (double)obj.Progress.TotalBytesToReceive;
        }

        private static async Task<StorageFile> GetLocalFileFromName(string filename)
        {
            StorageFile file = null;
            try
            {
                file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            }
            catch (FileNotFoundException)
            {
            }

            return file;
        }
    }
}
