using GalaSoft.MvvmLight.Messaging;
using HENG.BackgroundTasks;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
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
using Windows.UI.Notifications;

namespace HENG.Services
{
    public enum DownloadStartResult
    {
        Started,
        Error,
        AllreadyDownloaded,
    }

    public static class BackgroundTaskService
    {
        public static IEnumerable<BackgroundTask> BackgroundTasks => BackgroundTaskInstances.Value;

        private static readonly Lazy<IEnumerable<BackgroundTask>> BackgroundTaskInstances = new Lazy<IEnumerable<BackgroundTask>>(() => CreateInstances());

        private static ToastNotifier _notifier = ToastNotificationManager.CreateToastNotifier();

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

        public static async Task<DownloadStartResult> Download(Uri sourceUri)
        {
            var hash = SafeHashUri(sourceUri);
            var file = await CheckLocalFileExistsFromUriHash(sourceUri);

            var downloadingAlready = await IsDownloading(sourceUri);

            if (file == null && !downloadingAlready)
            {
                await Task.Run(() =>
                {
                    var task = StartDownload(sourceUri, BackgroundTransferPriority.High, hash);
                    task.ContinueWith((state) =>
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

        public static async Task CheckCompletionResult(IBackgroundTaskInstance instance)
        {
            var deferral = instance.GetDeferral();
            if (instance.TriggerDetails is BackgroundTransferCompletionGroupTriggerDetails trigger)
            {
                foreach (var item in trigger.Downloads)
                {
                    // TODO: handle other bad statuses
                    if (item.Progress.Status == BackgroundTransferStatus.Error)
                    {
                        await item.ResultFile.DeleteAsync();
                    }
                    else if (item.Progress.Status == BackgroundTransferStatus.Completed)
                    {
                        SetItemDownloaded(item);
                    }
                }
            }

            deferral?.Complete();
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

        private static void SetItemDownloaded(DownloadOperation item)
        {
            DownloadProgress(item);
        }

        public static async Task RegisterBackgroundTaskAsync()
        {
            BackgroundExecutionManager.RemoveAccess();
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            if (result == BackgroundAccessStatus.DeniedBySystemPolicy || result == BackgroundAccessStatus.DeniedByUser)
            {
                return;
            }

            foreach (var task in BackgroundTasks)
            {
                task.Register();
            }

            //var builder = new BackgroundTaskBuilder();
            //builder.Name = "DownloadCompleteTrigger";
            //builder.SetTrigger(trigger);

            //BackgroundTaskRegistration task = builder.Register();
        }

        private static async Task<StorageFile> CheckLocalFileExists(string fileName)
        {
            StorageFile file = null;
            try
            {
                var folder = await AppSettingService.GetDeaultDownloadPathAsync();
                file = await folder.GetFileAsync(fileName);
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
            StorageFile destinationFile = await GetLocalFileFromName(localFilename);
            BackgroundDownloader downloader = new BackgroundDownloader();
            CreateNotifications(downloader);
            DownloadOperation download = downloader.CreateDownload(target, destinationFile);
            download.Priority = priority;
            Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
            var downloadTask = download.StartAsync().AsTask(progressCallback);

            string tag = GetFileNameFromUri(target);

            CreateToast(tag, localFilename);

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
            Trace.WriteLine(obj.Progress.ToString());

            int progress = (int)(100 * (obj.Progress.BytesReceived / (double)obj.Progress.TotalBytesToReceive));

            string tag = GetFileNameFromUri(obj.RequestedUri);

            UpdateToast(obj.ResultFile.Name, progress);
        }

        private static async Task<StorageFile> GetLocalFileFromName(string filename)
        {
            StorageFile file = null;
            try
            {
                var folder = await AppSettingService.GetDeaultDownloadPathAsync();
                file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            }
            catch (FileNotFoundException)
            {
            }

            return file;
        }

        private static string GetFileNameFromUri(Uri sourceUri)
        {
            return Path.GetFileName(sourceUri.PathAndQuery);
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

        private static void CreateToast(string title, string tag)
        {
            ToastContent toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "IMG downloading...",
                            },

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
                { "progressValue", "0" }
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

        private static void UpdateToast(string toastTag, double progressValue)
        {
            var data = new Dictionary<string, string>
            {
                { "progressValue", progressValue.ToString() }
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

        private static IEnumerable<BackgroundTask> CreateInstances()
        {
            var backgroundTasks = new List<BackgroundTask>();

            backgroundTasks.Add(new DefaultBackgroundTask());
            return backgroundTasks;
        }
    }
}
