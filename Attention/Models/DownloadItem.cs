using GalaSoft.MvvmLight;
using PixabaySharp.Models;

namespace Attention.Models
{
    public class DownloadItem : ObservableObject
    {
        private ImageItem _item;
        public ImageItem Item
        {
            get { return _item; }
            private set { Set(ref _item, value); }
        }

        private double _Progress = 0.0d;
        public double Progress
        {
            get { return _Progress; }
            set { Set(ref _Progress, value); }
        }

        public DownloadItem(ImageItem item)
        {
            Item = item;
        }
    }
}
