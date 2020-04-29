using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Attention.App.Models
{
    public class WallpaperDto
    {
        public Brush Background { get; set; }
        public string Thumbnail { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
