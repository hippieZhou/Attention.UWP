using GalaSoft.MvvmLight.Messaging;
using HENG.UWP.Models;
using PixabaySharp.Models;
using System.Collections.Generic;

namespace HENG.UWP.Services
{
    /// <summary>
    /// https://medium.com/@jeremiecorpinot/uwp-build-a-download-progress-8a5f21337b61
    /// </summary>
    public class DownloadService
    {
        private readonly List<DownloadItem> _histories = new List<DownloadItem>();

        public IEnumerable<DownloadItem> GetHistories() => _histories;

        public void SaveToPicturesLibrary(ImageItem item)
        {
            var download = new DownloadItem(item);
            _histories.Add(download);
            Messenger.Default.Send(download, nameof(DownloadService));
        }
    }
}
