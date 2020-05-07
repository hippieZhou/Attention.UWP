using System;
using Windows.UI.Xaml.Media;

namespace Attention.App.Models
{
    public class WallpaperDto
    {
        public Brush Background { get; set; }
        public string Thumbnail { get; set; }
        public string ImageUri { get; set; }
        public string ImageAuthor { get; set; }
        public string ImageAuthorUrl { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
