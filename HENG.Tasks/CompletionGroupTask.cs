using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace HENG.Tasks
{
    public sealed class CompletionGroupTask: IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            if (!(taskInstance.TriggerDetails is BackgroundTransferCompletionGroupTriggerDetails details))
            {
                deferral.Complete();
                return;
            }


            List<DownloadOperation> failedDownloads = new List<DownloadOperation>();
            int succeded = 0;
            foreach (DownloadOperation download in details.Downloads)
            {
                if (IsFailed(download))
                {
                    failedDownloads.Add(download);
                }
                else
                {
                    succeded++;
                }
            }

            if (failedDownloads.Count > 0)
            {
                await RetryDownloadsAsync(failedDownloads);
            }

            deferral.Complete();

        }

        private bool IsFailed(DownloadOperation download)
        {
            BackgroundTransferStatus status = download.Progress.Status;
            if (status == BackgroundTransferStatus.Error || status == BackgroundTransferStatus.Canceled)
            {
                return true;
            }

            ResponseInformation response = download.GetResponseInformation();
            if (response == null || response.StatusCode != 200)
            {
                return true;
            }

            return false;
        }

        private async Task RetryDownloadsAsync(IEnumerable<DownloadOperation> downloadsToRetry)
        {
            BackgroundDownloader downloader = CreateBackgroundDownloader();

            foreach (var downloadToRetry in downloadsToRetry)
            {
                string originalName = downloadToRetry.ResultFile.Name;
                string newName = originalName.Insert(originalName.LastIndexOf('.'), "_retried");
                await downloadToRetry.ResultFile.RenameAsync(newName, NameCollisionOption.ReplaceExisting);

                DownloadOperation download = downloader.CreateDownload(downloadToRetry.RequestedUri, downloadToRetry.ResultFile);
                Task<DownloadOperation> startTask = download.StartAsync().AsTask();
            }

            downloader.CompletionGroup.Enable();
        }


        public static BackgroundDownloader CreateBackgroundDownloader()
        {
            BackgroundTransferCompletionGroup completionGroup = new BackgroundTransferCompletionGroup();

            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.TaskEntryPoint = "Tasks.CompletionGroupTask";
            builder.SetTrigger(completionGroup.Trigger);

            // The system automatically unregisters the BackgroundTransferCompletionGroup task when it triggers.
            // You do not need to unregister it explicitly.
            BackgroundTaskRegistration taskRegistration = builder.Register();

            BackgroundDownloader downloader = new BackgroundDownloader(completionGroup);

            return downloader;
        }
    }
}
