using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HENG.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace HENG.Models
{
    public class DownloadItem : ObservableObject
    {
        private readonly CancellationTokenSource cts;

        private Uri _requestedUri;
        public Uri RequestedUri
        {
            get { return _requestedUri; }
            private set { Set(ref _requestedUri, value); }
        }

        private IStorageFile _resultFile;
        public IStorageFile ResultFile
        {
            get { return _resultFile; }
            set { Set(ref _resultFile, value); }
        }

        private object _photo;
        public object Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { Set(ref _progress, value); }
        }

        public DownloadItem(Uri uri)
        {
            RequestedUri = uri;
            Photo = RequestedUri.AbsoluteUri;
            cts = new CancellationTokenSource();

            Messenger.Default.Register<Tuple<IStorageFile, int>>(this, RequestedUri, async todo => 
            {
                ResultFile = todo.Item1;
                Progress = todo.Item2;
                if (Progress >= 100)
                {
                    Photo = await ImageHelper.StorageFileToBitmapImage(ResultFile);
                }
            });
        }

        public DownloadItem()
        {

        }

        public async Task DownloadAsync()
        {
            await BackgroundDownloadHelper.DownLoad(RequestedUri, cts);
        }

        public void Cancel()
        {
            cts.Cancel();
        }
    }
}
