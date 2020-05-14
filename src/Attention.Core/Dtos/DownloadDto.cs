using System;
using Windows.UI.Xaml.Media;

namespace Attention.Core.Dtos
{
    public class DownloadDto
    {
        public string Title { get; set; }
        public Brush Background { get; set; }
        public string Thumbnail { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
