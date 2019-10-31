using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Attention.UWP.Extensions;
using Attention.UWP.Models.Core;
using Attention.UWP.Models.Repositories;
using Attention.UWP.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MetroLog;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using PixabaySharp.Models;
using Windows.ApplicationModel.Background;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
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
        private readonly IRepository<Download> _repository;
        private readonly StorageFolder _folder;
        private readonly Download _entity;
        private CancellationTokenSource cts;

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get
            {
                if (_imageSource == null)
                {
                    _imageSource = new BitmapImage(new Uri(_entity.ImageURL));
                }
                return _imageSource;
            }
            set { Set(ref _imageSource, value); }
        }

        #region Ctors
        private DownloadItem(StorageFolder folder)
        {
            _logger = ViewModelLocator.Current.LogManager.GetLogger<DownloadItem>();
            _repository = ViewModelLocator.Current.DAL.DownloadRepo;
            _folder = folder;
        }
        public DownloadItem(ImageItem item, StorageFolder folder) : this(folder)
        {
            var imgUrl = string.IsNullOrEmpty(item.FullHDImageURL?.Trim()) ? item.LargeImageURL : item.FullHDImageURL;
            _entity = new Download()
            {
                Model = item,
                Json = JsonConvert.SerializeObject(item),
                ImageURL = imgUrl,
                FileName = GetFileNameFromUri(new Uri(imgUrl))
            };
        }
        public DownloadItem(Download entity, StorageFolder folder) : this(folder)
        {
            _entity = entity;
            _entity.Model = JsonConvert.DeserializeObject<ImageItem>(_entity.Json);
        }
        #endregion

        public async Task<DownloadItemResult> DownloadAsync()
        {
            Uri sourceUri = new Uri(_entity.ImageURL);
            StorageFile file = await CheckLocalFileExists(_folder, _entity.FileName);
            bool downloadingAlready = await IsDownloading(sourceUri);
            if (file == null && !downloadingAlready)
            {
                _entity.Id = await SaveItemDownloaded(_repository, _entity);

                await Task.Run(() =>
                {
                    Task task = StartDownload(sourceUri, BackgroundTransferPriority.High);
                    task.ContinueWith(async (state) =>
                      {
                          if (state.Exception != null)
                          {
                              _logger.Error($"An error occured with this download {state.Exception}", state.Exception);
                          }
                          else
                          {
                              Debug.WriteLine("Download Completed");
                              await RefreshImageSource();
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

        public async Task RefreshImageSource()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                var source = await LocalFileToImage(_folder, _entity.FileName);
                if (source != default)
                {
                    ImageSource = source;
                }
            });
        }

        public void Cancel()
        {
            cts?.Cancel();
            cts?.Dispose();
        }

        public async Task DeleteAsync() => await DeleteItemDownloaded(_repository, _entity, _folder);

        private async Task StartDownload(Uri target, BackgroundTransferPriority priority)
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            BackgroundDownloader downloader = new BackgroundDownloader();

            CreateNotifications(downloader);
            StorageFile destinationFile = await GetLocalFileFromName(_folder, _entity.FileName);
            DownloadOperation download = downloader.CreateDownload(target, destinationFile);
            download.Priority = priority;

            Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(obj =>
            {
                Debug.WriteLine($"{obj.Progress.Status}:{obj.Progress.ToString()}");

                var progress = obj.Progress.BytesReceived / (double)obj.Progress.TotalBytesToReceive;
                UpdateToast(progress, obj.ResultFile.Name);
            });

            if (cts == default)
            {
                cts = new CancellationTokenSource();
            }
            var downloadTask = download.StartAsync().AsTask(cts.Token, progressCallback);

            CreateToast(_folder.Path, _entity.Model.UserImageURL, _entity.Model.User, _entity.Model.PreviewURL, _entity.FileName);
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
        private static readonly ToastNotifier _notifier = ToastNotificationManager.CreateToastNotifier();
        private static async Task<bool> IsDownloading(Uri uri)
        {
            var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            return downloads.Any(dl => dl.RequestedUri == uri);
        }
        private static async Task<StorageFile> GetLocalFileFromName(StorageFolder folder, string fileName)
        {
            return await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        }
        private static string GetFileNameFromUri(Uri sourceUri)
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

            var fi = new FileInfo(Path.GetFileName(sourceUri.PathAndQuery));
            return Hash(fi.Name) + fi.Extension;
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
        private static async Task<BitmapImage> LocalFileToImage(StorageFolder folder, string fileName)
        {
            if (await folder.TryGetItemAsync(fileName) is IStorageFile file)
            {
                var bytes = await file.AsByteArray();
                return bytes.AsBitmapImage();
            }
            else
            {
                return default;
            }
        }
        private static void CreateNotifications(BackgroundDownloader downloader)
        {
            var successToastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            successToastXml.GetElementsByTagName("text").Item(0).InnerText =
                "Downloads completed successfully.";
            ToastNotification successToast = new ToastNotification(successToastXml);
            downloader.SuccessToastNotification = successToast;

            var failureToastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            failureToastXml.GetElementsByTagName("text").Item(0).InnerText =
                "At least one download completed with failure.";
            ToastNotification failureToast = new ToastNotification(failureToastXml);
            downloader.FailureToastNotification = failureToast;
        }
        private static void CreateToast(string title,string userLogo,string userName, string heroImage, string tag)
        {
            ToastContent toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Attribution = new ToastGenericAttributionText()
                        {
                            Text = "Via pixabay",
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            AddImageQuery = true,
                            Source = userLogo,
                            AlternateText = userName,
                            HintCrop = ToastGenericAppLogoCrop.Circle,
                        },
                        HeroImage = new ToastGenericHeroImage()
                        {
                            Source = heroImage,
                        },
                        Children =
                        {
                            new AdaptiveProgressBar()
                            {
                                Title = title,
                                Value = new BindableProgressBarValue("progressValue"),
                                Status = "Downloading...",
                            },
                        },
                    },
                },
            };

            var data = new Dictionary<string, string>
            {
                { "progressValue", "0" },
            };

            // And create the toast notification
            ToastNotification notification = new ToastNotification(toastContent.GetXml())
            {
                Tag = tag,
                Data = new NotificationData(data),
            };

            // And then send the toast
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }
        private static void UpdateToast(double progressValue, string toastTag)
        {
            var data = new Dictionary<string, string>
            {
                { "progressValue", progressValue.ToString() },
            };

            try
            {
                _notifier.Update(new NotificationData(data), toastTag);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        private static async Task<int> SaveItemDownloaded(IRepository<Download> _repository, Download download)
        {
            await _repository.InsertAsync(download);
            using (Task<int> idTask = await _repository.GetAllAsync().ContinueWith(
                async task =>
                {
                    var items = await task;
                    return items.FirstOrDefault(p => p.FileName == download.FileName).Id;
                }))
            {
                return await idTask;
            }
        }
        private static async Task DeleteItemDownloaded(IRepository<Download> repository, Download download, StorageFolder folder)
        {
            await repository.DeleteAsync(download);
            if (await folder.TryGetItemAsync(download.FileName) is IStorageFile file)
            {
                await file.DeleteAsync();
            }
        }
    }
}
