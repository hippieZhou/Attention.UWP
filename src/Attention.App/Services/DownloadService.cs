using Attention.Core.Services;
using Prism.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace Attention.App.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly ILoggerFacade _logger;

        public DownloadService(ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Download(string folderName, string fileName, string downloadUri)
        {
            Uri sourceUri = new Uri(downloadUri);
            StorageFile file = await CheckLocalFileExists(folderName, fileName);
            bool downloadingAlready = await IsDownloading(sourceUri);
            if (file == null && !downloadingAlready)
            {
                await Task.Run(() =>
                {
                    Task task = StartDownload(sourceUri, BackgroundTransferPriority.High);
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
            }
        }

        private Task StartDownload(Uri sourceUri, BackgroundTransferPriority priority)
        {
            //var result = await BackgroundExecutionManager.RequestAccessAsync();

            //BackgroundTransferCompletionGroup completionGroup = new BackgroundTransferCompletionGroup();
            //RegisterBackgroundTask(completionGroup.Trigger);

            //BackgroundDownloader downloader = new BackgroundDownloader(completionGroup);

            //var group = BackgroundTransferGroup.CreateGroup(Guid.NewGuid().ToString());
            //group.TransferBehavior = BackgroundTransferBehavior.Serialized;
            //downloader.TransferGroup = group;

            //CreateNotifications(downloader);
            //StorageFile destinationFile = await GetLocalFileFromName(_folder, _entity.FileName);
            //DownloadOperation download = downloader.CreateDownload(target, destinationFile);
            //download.Priority = priority;

            //completionGroup.Enable();

            //if (cts == default)
            //{
            //    cts = new CancellationTokenSource();
            //}

            //Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
            //var downloadTask = download.StartAsync().AsTask(cts.Token, progressCallback);

            ////CreateToast(_folder.Path, _entity.Model.UserImageURL, _entity.Model.User, _entity.Model.PreviewURL, _entity.FileName);
            //try
            //{
            //    await downloadTask;

            //    // Will occur after download completes
            //    ResponseInformation response = download.GetResponseInformation();
            //}
            //catch (Exception)
            //{
            //    _logger.Log("Download exception", Category.Debug, Priority.None);
            //}

            throw new NotImplementedException();
        }

        #region InnerCode
        private static async Task<StorageFile> CheckLocalFileExists(string folder, string fileName)
        {
            var sf = await StorageFolder.GetFolderFromPathAsync(folder);
            StorageFile file = await sf.TryGetItemAsync(fileName) as StorageFile;
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
            return downloads.Any(dl => dl.RequestedUri == sourceUri);
        }
        #endregion
    }
}
