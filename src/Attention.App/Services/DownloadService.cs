using Attention.Core.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Notifications;

namespace Attention.App.Services
{

    public class DownloadService : IDownloadService
    {
        private readonly ILoggerFacade _logger;

        public DownloadService(ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DownloadResult> Download(string folderName, string fileName, string downloadUri, CancellationToken cancellationToken)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(folderName);
            StorageFile file = await CheckLocalFileExists(folder, fileName);

            Uri sourceUri = new Uri(downloadUri);
            bool downloadingAlready = await IsDownloading(sourceUri);

            if (file == null && !downloadingAlready)
            {
                await Task.Run(() =>
                {
                    Task task = StartDownload(folder, fileName, sourceUri, BackgroundTransferPriority.High, cancellationToken);
                    task.ContinueWith((state) =>
                    {
                        if (state.Exception != null)
                        {
                            _logger.Log($"An error occured with this download {state.Exception}", Category.Exception, Priority.High);
                            throw new Exception($"An error occured with this download {state.Exception}", state.Exception);
                        }
                        else
                        {
                            _logger.Log("Download Completed", Category.Debug, Priority.None);
                        }
                    });
                });

                return DownloadResult.Started;
            }
            else if (file != null)
            {
                return DownloadResult.AllreadyDownloaded;
            }
            else
            {
                return DownloadResult.Error;
            }
        }

        private async Task StartDownload(
            StorageFolder folder, string fileName, Uri target,
            BackgroundTransferPriority priority, CancellationToken cancellationToken)
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            BackgroundTransferCompletionGroup completionGroup = new BackgroundTransferCompletionGroup();
            RegisterBackgroundTask(completionGroup.Trigger);

            BackgroundDownloader downloader = new BackgroundDownloader(completionGroup);

            var group = BackgroundTransferGroup.CreateGroup(Guid.NewGuid().ToString());
            group.TransferBehavior = BackgroundTransferBehavior.Serialized;
            downloader.TransferGroup = group;

            CreateNotifications(downloader);

            StorageFile destinationFile = await GetLocalFileFromName(folder, fileName);
            DownloadOperation download = downloader.CreateDownload(target, destinationFile);
            download.Priority = priority;

            completionGroup.Enable();

            Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
            var downloadTask = download.StartAsync().AsTask(cancellationToken, progressCallback);

            CreateToast(fileName);
            try
            {
                await downloadTask;

                // Will occur after download completes
                ResponseInformation response = download.GetResponseInformation();
            }
            catch (Exception ex)
            {
                _logger.Log($"Download exception:{ex}", Category.Debug, Priority.None);
            }
        }

        private void DownloadProgress(DownloadOperation obj)
        {
            _logger.Log($"{obj.Progress.Status}:{obj.Progress}", Category.Debug, Priority.None);

            var progress = obj.Progress.BytesReceived / (double)obj.Progress.TotalBytesToReceive;

            try
            {
                UpdateToast(progress, obj.ResultFile.Name);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.ToString(), Category.Exception, Priority.High);
            }
        }

        #region Private Methods

        private async Task<bool> IsDownloading(Uri sourceUri)
        {
            var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            return downloads.Any(dl => dl.RequestedUri == sourceUri);
        }
        #endregion

        #region Static Methods
        private static readonly ToastNotifier _notifier = ToastNotificationManager.CreateToastNotifier();

        private static void UpdateToast(double progressValue, string toastTag)
        {
            var data = new Dictionary<string, string>
            {
                { "progressValue", progressValue.ToString() },
            };
            _notifier.Update(new NotificationData(data), toastTag);
        }
        private static async Task<StorageFile> GetLocalFileFromName(StorageFolder folder, string fileName)
        {
            return await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        }
        private static void CreateToast(string tag, string title = default, string userAvatar = default, string userName = default, string heroImage = default)
        {
            ToastContent toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Attribution = new ToastGenericAttributionText()
                        {
                            Text = "Via unsplash.com",
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            AddImageQuery = true,
                            Source = userAvatar,
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
        private static void RegisterBackgroundTask(IBackgroundTrigger trigger)
        {
            var builder = new BackgroundTaskBuilder
            {
                Name = "DownloadCompleteTrigger"
            };
            builder.SetTrigger(trigger);

            BackgroundTaskRegistration task = builder.Register();
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
        #endregion
    }
}
