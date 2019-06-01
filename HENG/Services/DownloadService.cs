using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HENG.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Notifications;

namespace HENG.Services
{
    public enum DownloadStartResult
    {
        Started,
        Error,
        AllreadyDownloaded,
    }

    public class DownloadService
    {
        private static readonly ToastNotifier _notifier = ToastNotificationManager.CreateToastNotifier();

        private static async Task StartDownloadAsync(string url, string hash, CancellationTokenSource cancellationToken)
        {
            StorageFile file = await CheckLocalFileExistsAsync(hash) as StorageFile;
            if (file == null)
            {
                StorageFolder folder = await KnownFolders.PicturesLibrary.GetFolderAsync("HENG");
                file = await folder.CreateFileAsync(hash, CreationCollisionOption.ReplaceExisting);
            }

            Uri downloadUrl = new Uri(url);

            BackgroundDownloader downloader = new BackgroundDownloader();
            CreateNotifications(downloader);

            var downloadOperation = downloader.CreateDownload(downloadUrl, file);
            Progress<DownloadOperation> progress = new Progress<DownloadOperation>(x => ProgressChanged(downloadOperation));
            Trace.WriteLine("Initializing...");
            var downloadTask = downloadOperation.StartAsync().AsTask(cancellationToken.Token, progress);
            CreateToast(file.Name, downloadUrl.AbsoluteUri);
            try
            {
                await downloadTask;
                ResponseInformation response = downloadOperation.GetResponseInformation();
            }
            catch (TaskCanceledException)
            {
                Trace.WriteLine("Download canceled.");
                await downloadOperation.ResultFile.DeleteAsync();
                downloadOperation = null;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
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

        private static void CreateToast(string title, string sourceUri)
        {
            ToastContent toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        HeroImage = new ToastGenericHeroImage
                        {
                            Source = sourceUri
                        },
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "File downloading...",
                            },

                            new AdaptiveProgressBar()
                            {
                                Title = title,
                                Value = new BindableProgressBarValue("progressValue"),
                                ValueStringOverride = new BindableString("p"),
                                Status = "Downloading...",
                            },
                        },
                    },
                },
            };
            string now = DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var data = new Dictionary<string, string>
            {
                { "progressValue", "0" },
                { "p", now },
            };

            ToastNotification notification = new ToastNotification(toastContent.GetXml())
            {
                Tag = title,
                Data = new NotificationData(data),
            };

            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }

        private static void ProgressChanged(DownloadOperation downloadOperation)
        {
            int progress = (int)(100 * (downloadOperation.Progress.BytesReceived / (double)downloadOperation.Progress.TotalBytesToReceive));
            UpdateToast(downloadOperation.ResultFile.Name, progress);
        }

        private static void UpdateToast(string toastTag, int progressValue)
        {
            string now = DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture);

            var data = new Dictionary<string, string>
            {
                { "progressValue", progressValue.ToString() },
                { "p", now }
            };

            try
            {
                _notifier.Update(new NotificationData(data), toastTag);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
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

        private static async Task<IStorageItem> CheckLocalFileExistsAsync(string hash)
        {
            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("HENG", CreationCollisionOption.OpenIfExists);
            IStorageItem file = await folder.TryGetItemAsync(hash);
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

        public static async Task AttachToDownloadsAsync(CancellationTokenSource cancellationToken)
        {
            IReadOnlyList<DownloadOperation> downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            foreach (DownloadOperation download in downloads)
            {
                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(ProgressChanged);
                await download.AttachAsync().AsTask(cancellationToken.Token, progressCallback);
            }
        }

        public static async Task<DownloadStartResult> DownloadAsync(DownloadItem download)
        {
            IStorageItem file = await CheckLocalFileExistsAsync(download.Hash);
            var downloadingAlready = await IsDownloading(new Uri(download.Item.LargeImageURL));
            if (file == null && !downloadingAlready)
            {
                await Task.Run(() =>
                {
                    var task = StartDownloadAsync(download.Item.LargeImageURL, download.Hash, download.CancellationToken);
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
                return DownloadStartResult.Started;
            }
            else if (file != null)
            {
                Trace.WriteLine("Already downloaded.");
                return DownloadStartResult.AllreadyDownloaded;
            }
            else
            {
                return DownloadStartResult.Error;
            }
        }
    }
}
