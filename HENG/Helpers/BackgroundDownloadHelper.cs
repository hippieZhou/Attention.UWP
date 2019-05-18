using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Notifications;

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
        private static readonly ToastNotifier _notifier = ToastNotificationManager.CreateToastNotifier();

        public static async Task AttachToDownloadsAsync(CancellationTokenSource cancellationToken)
        {
            IReadOnlyList<DownloadOperation> downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            foreach (DownloadOperation download in downloads)
            {
                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                await download.AttachAsync().AsTask(cancellationToken.Token, progressCallback);
            }
        }

        public static async Task<DownloadStartResult> DownLoad(Uri sourceUri, CancellationTokenSource cancellationToken)
        {
            var hash = SafeHashUri(sourceUri);
            StorageFile file = await CheckLocalFileExists(hash);

            var downloadingAlready = await IsDownloading(sourceUri);
            if (file == null && !downloadingAlready)
            {
                await Task.Run(() =>
                {
                    var task = StartDownloadAsync(sourceUri, BackgroundTransferPriority.High, hash, cancellationToken);
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

        private static async Task<StorageFile> CheckLocalFileExists(string fileName)
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

        private static async Task<bool> IsDownloading(Uri sourceUri)
        {
            var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            if (downloads.Where(dl => dl.RequestedUri == sourceUri).FirstOrDefault() != null)
            {
                return true;
            }

            return false;
        }

        private static async Task StartDownloadAsync(Uri sourceUri, BackgroundTransferPriority priority, string localFilename, CancellationTokenSource cancellationToken)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(App.Settings.DownloadPath);
            StorageFile destinationFile = await folder.CreateFileAsync(localFilename, CreationCollisionOption.ReplaceExisting);

            var group = BackgroundTransferGroup.CreateGroup(Guid.NewGuid().ToString());
            group.TransferBehavior = BackgroundTransferBehavior.Serialized;
            BackgroundTransferCompletionGroup completionGroup = new BackgroundTransferCompletionGroup();
            BackgroundTaskRegistration taskRegistration = RegisterBackgroundTask(completionGroup.Trigger);
            BackgroundDownloader downloader = new BackgroundDownloader
            {
                TransferGroup = group
            };
            CreateNotifications(downloader);

            DownloadOperation download = downloader.CreateDownload(sourceUri, destinationFile);
            download.Priority = priority;

            completionGroup.Enable();

            Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
            var downloadTask = download.StartAsync().AsTask(cancellationToken.Token, progressCallback);

            CreateToast(localFilename, localFilename, sourceUri.AbsoluteUri);

            try
            {
                await downloadTask;
                ResponseInformation response = download.GetResponseInformation();
            }
            catch (TaskCanceledException)
            {
                await download.ResultFile.DeleteAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Download exception:{ex}");
            }
        }

        private static BackgroundTaskRegistration RegisterBackgroundTask(IBackgroundTrigger trigger)
        {
            var builder = new BackgroundTaskBuilder
            {
                Name = "DownloadCompleteTrigger"
            };
            builder.SetTrigger(trigger);

            BackgroundTaskRegistration task = builder.Register();
            return task;
        }

        private static void DownloadProgress(DownloadOperation obj)
        {
            int progress = (int)(100 * (obj.Progress.BytesReceived / (double)obj.Progress.TotalBytesToReceive));
            Trace.WriteLine(progress);

            UpdateToast(obj.ResultFile.Name, progress);
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

        private static void CreateToast(string title, string tag,string sourceUri)
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
                Tag = tag,
                Data = new NotificationData(data),
            };

            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }

        private static void UpdateToast(string toastTag, double progressValue)
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
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
