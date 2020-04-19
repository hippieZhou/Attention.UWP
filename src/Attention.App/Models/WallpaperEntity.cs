using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Attention.App.Models
{
    public class WallpaperEntity
    {
        public Brush Background { get; set; }
        public DateTime CreateAt { get; set; }
        public BitmapImage Thumbnail { get; set; }
    }
}
