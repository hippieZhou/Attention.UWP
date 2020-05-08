using Attention.Core.Mappings;
using AutoMapper;
using PixabaySharp.Models;
using System;
using Unsplasharp.Models;
using Windows.UI.Xaml.Media;

namespace Attention.Core.Dtos
{
    public class WallpaperDto : IMapFrom<ImageItem>, IMapFrom<Photo>
    {
        public Brush Background { get; set; }
        public string Thumbnail { get; set; }
        public string ImageUri { get; set; }
        public string ImageAuthor { get; set; }
        public string ImageAuthorUrl { get; set; }
        public DateTime Created { get; set; }

        public void Mapping(Profile profile)
        {
        }
    }
}
