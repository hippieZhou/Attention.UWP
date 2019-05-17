using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HENG.Clients;
using HENG.Models;
using HENG.ViewModels;
using Microsoft.Toolkit.Uwp.UI;
using System.Linq;
using Newtonsoft.Json;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Networking.BackgroundTransfer;
using System.Diagnostics;

namespace HENG.Services
{
    public partial class DataService
    {
        static DataService()
        {
            ImageCache.Instance.MaxMemoryCacheCount = 1000;
            ImageCache.Instance.CacheDuration = TimeSpan.FromHours(24);
        }

        public async Task GetFromCacheAsync(string url, Action<BitmapImage> action)
        {
            var file = await ImageCache.Instance.GetFileFromCacheAsync(new Uri(url));
            if (file != null)
            {
                var props = await file.GetBasicPropertiesAsync();
                if (props.Size == 0)
                {
                    await file.DeleteAsync();
                }
            }
            var task = ImageCache.Instance.PreCacheAsync(new Uri(url));
            await task.ContinueWith(async t =>
            {
                var bmp = await ImageCache.Instance.GetFromCacheAsync(new Uri(url));
                action(bmp);
            }).ConfigureAwait(false); ;
        }

        public async Task<StorageFile> GetFileFromCacheAsync(string url)
        {
            var sf = await ImageCache.Instance.GetFileFromCacheAsync(new Uri(url));
            return sf;
        }
    }
    public partial class DataService
    {
        private IBaseClient<BingItem> Home_Client => ViewModelLocator.Current.ServiceProvider.GetService(typeof(BingClient)) as IBaseClient<BingItem>;
        private IBaseClient<PicsumItem> Picsum_Client => ViewModelLocator.Current.ServiceProvider.GetService(typeof(PicsumClient)) as IBaseClient<PicsumItem>;
        private PaperClient Paper_Client => ViewModelLocator.Current.ServiceProvider.GetService(typeof(PaperClient)) as PaperClient;

        public async Task<IEnumerable<BingItem>> GetItemsForBingAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Home_Client?.GetItemsAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }

        public async Task<IEnumerable<PicsumItem>> GetItemsForPicsumAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Picsum_Client?.GetItemsAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetItemsForNewestAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Paper_Client?.GetNewestAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetItemsForHottestAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Paper_Client?.GetHottestAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetItemsForGirlsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Paper_Client?.GetGirlsAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetItemsForSkylandAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Paper_Client?.GetSkylandAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }
    }

    public partial class DataService
    {
        public List<DownloadOperation> ActiveDownloads { get; private set; } = new List<DownloadOperation>();

        public async Task DownLoad(Uri sourceUri)
        {
            var hash = SafeHashUri(sourceUri);
            StorageFile file = await CheckLocalFileExists(hash);

            var downloadingAlready = await IsDownloading(sourceUri);
            if (file == null && !downloadingAlready)
            {
                await Task.Run(() => 
                {
                    var task = StartDownloadAsync(sourceUri, BackgroundTransferPriority.High, hash);
                    task.ContinueWith(state => 
                    {
                        if (state.Exception != null)
                        {
                            Trace.WriteLine($"An error occured with this download {state.Exception}");
                        }
                        else
                        {
                            Trace.WriteLine("Download Completed");
                        }
                    });
                });
            }
        }

        private string SafeHashUri(Uri sourceUri)
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

        private async Task<StorageFile> CheckLocalFileExists(string fileName)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(App.Settings.DownloadPath);
            if (folder == null)
                throw new Exception($"{App.Settings.DownloadPath} Path Not Found.");
            StorageFile file = await folder.TryGetItemAsync(fileName) as StorageFile;
            if (file != null)
            {
                var props = await file.GetBasicPropertiesAsync();
                if (props.Size == 0)
                {
                    await file.DeleteAsync();
                    return null;
                }
            }
            return file;
        }

        private async Task<bool> IsDownloading(Uri sourceUri)
        {
            var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            if (downloads.Where(dl => dl.RequestedUri == sourceUri).FirstOrDefault() != null)
            {
                return true;
            }

            return false;
        }

        private async Task StartDownloadAsync(Uri sourceUri, BackgroundTransferPriority priority, string localFilename)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(App.Settings.DownloadPath);
            StorageFile destinationFile = await folder.CreateFileAsync(localFilename, CreationCollisionOption.ReplaceExisting);

            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(sourceUri, destinationFile);
            download.Priority = priority;
            Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(obj =>
            {
                Trace.WriteLine(obj.Progress.ToString());
                int progress = (int)(100 * (obj.Progress.BytesReceived / (double)obj.Progress.TotalBytesToReceive));
            });

            ActiveDownloads.Add(download);
            var downloadTask = download.StartAsync().AsTask(progressCallback);

            try
            {
                await downloadTask;
                ResponseInformation response = download.GetResponseInformation();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Download exception:{ex}");
            }
            finally
            {
                ActiveDownloads.Remove(download);
            }
        }
    }
}
