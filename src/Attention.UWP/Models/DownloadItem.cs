using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Attention.UWP.Extensions;
using Attention.UWP.Models.Core;
using Attention.UWP.Services;
using Attention.UWP.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MetroLog;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using PixabaySharp.Models;
using Windows.ApplicationModel.Background;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Attention.UWP.Models
{
    public enum DownloadItemResult
    {
        Started,
        Error,
        AllreadyDownloaded,
    }

    public partial class DownloadItem : ObservableObject
    {
        private readonly ILogger _logger;
        private readonly StorageFolder _folder;
        private readonly Download _entity;

        public DownloadItem(StorageFolder folder)
        {
            _folder = folder;
            _logger = ViewModelLocator.Current.LogManager.GetLogger<DownloadItem>();
        }
        public DownloadItem(Download entity, StorageFolder folder) : this(folder) => _entity = entity;
        public DownloadItem(ImageItem item, StorageFolder folder) : this(folder)
        {
            _entity = new Download()
            {
                Json = JsonConvert.SerializeObject(item),
                ImageUrl = string.IsNullOrEmpty(item.FullHDImageURL?.Trim()) ? item.LargeImageURL : item.FullHDImageURL,
            };
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get
            {
                if (_imageSource == null)
                {
                    _imageSource = new BitmapImage(new Uri(_entity.ImageUrl));
                }
                return _imageSource;
            }
            set { Set(ref _imageSource, value); }
        }

        /// <summary>
        /// https://github.com/jQuery2DotNet/UWP-Samples/blob/master/BackgroundDownloader/BackgroundDownloader/MainPage.xaml.cs
        /// </summary>
        /// <returns></returns>
        public async Task<DownloadItemResult> StartAsync()
        {
            async Task<bool> IsDownloading(Uri uri)
            {
                var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
                return downloads.Any(dl => dl.RequestedUri == uri);
            }

            var sourceUri = new Uri(_entity.ImageUrl);
            var file = await CheckLocalFileExistsFromUriHash(sourceUri, _folder);
            var downloadingAlready = await IsDownloading(sourceUri);

            if (file == null && !downloadingAlready)
            {
                _entity.FileName = SafeHashUri(sourceUri);
                await DAL.SaveOrUpdateAsync(_entity);

                await Task.Run(() =>
                {
                    var task = StartDownload(sourceUri, BackgroundTransferPriority.High, _entity.FileName);
                    task.ContinueWith(async (state) =>
                      {
                          if (state.Exception != null)
                          {
                              _logger.Error($"An error occured with this download {state.Exception}", state.Exception);
                          }
                          else
                          {
                              Debug.WriteLine("Download Completed");
                              await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                               {
                                   ImageSource = await LocalFileToImage(_folder, _entity.FileName);
                               });
                          }
                      });
                });
                Messenger.Default.Send(this, nameof(DownloadItem));
                return DownloadItemResult.Started;
            }
            else if (file != null)
            {
                return DownloadItemResult.AllreadyDownloaded;
            }
            else
            {
                return DownloadItemResult.Error;
            }
        }

        public async Task DeleteAsync()
        {
            await DAL.DeleteAsync(_entity);
            if (await _folder.TryGetItemAsync(_entity.FileName) is IStorageFile file)
            {
                await file.DeleteAsync();
            }
        }

        private async Task StartDownload(Uri target, BackgroundTransferPriority priority, string localFilename)
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            StorageFile destinationFile = await GetLocalFileFromName(_folder, localFilename);

            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(target, destinationFile);
            download.Priority = priority;

            Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(obj =>
            {
                Debug.WriteLine(obj.Progress.ToString());

                var progress = obj.Progress.BytesReceived / (double)obj.Progress.TotalBytesToReceive;
            });
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
    }

    public partial class DownloadItem : ObservableObject
    {
        private static string SafeHashUri(Uri sourceUri)
        {
            string safeUri = sourceUri.ToString().ToLower();
            var hash = Hash(safeUri);
            string suffix = sourceUri.Segments.LastOrDefault()?.Split(".").LastOrDefault() ?? ".jpg";
            return $"{hash}.{suffix}";
        }
        private static async Task<StorageFile> CheckLocalFileExistsFromUriHash(Uri sourceUri, StorageFolder folder)
        {
            string hash = SafeHashUri(sourceUri);
            return await CheckLocalFileExists(folder, hash);
        }
        private static async Task<StorageFile> GetLocalFileFromName(StorageFolder folder, string filename)
        {
            return await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        }
        private static async Task<StorageFile> CheckLocalFileExists(StorageFolder folder, string fileName)
        {
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
        private static async Task<BitmapImage> LocalFileToImage(StorageFolder folder, string filename)
        {
            if (await folder.TryGetItemAsync(filename) is IStorageFile file)
            {
                var bytes = await file.AsByteArray();
                return bytes.AsBitmapImage();
            }
            else
            {
                return default;
            }
        }
        private static string Hash(string input)
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
    }
}
