using Attention.Core.Mappings;
using AutoMapper;
using PixabaySharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unsplasharp.Models;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Attention.Core.Dtos
{
    public class WallpaperDto : IMapFrom<ImageItem>, IMapFrom<Photo>
    {
        private readonly static Color[] colors = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null)).ToArray();
        public static IEnumerable<WallpaperDto> FakeData => colors.Select(x => new WallpaperDto
        {
            Background = new AcrylicBrush
            {
                BackgroundSource = AcrylicBackgroundSource.Backdrop,
                TintColor = x,
                FallbackColor = x,
                TintOpacity = 1.0,
                TintLuminosityOpacity = 1.0,
            },
            ImageAuthor = DateTime.Now.Year.ToString(),
            ImageAuthorUrl = "https://www.bing.com",
            ImageUri = "ms-appx:///Assets/Images/bantersnaps-wPMvPMD9KBI-unsplash.jpg",
            Thumbnail = "ms-appx:///Assets/Images/bantersnaps-wPMvPMD9KBI-unsplash.jpg"
        });

        public Brush Background { get; set; }
        public string Thumbnail { get; set; }
        public string ImageUri { get; set; }
        public string ImageAuthor { get; set; }
        public string ImageAuthorUrl { get; set; }

        public void Mapping(Profile profile)
        {
            var random = new Random(DateTime.Now.Millisecond);

            profile.CreateMap<ImageItem, WallpaperDto>()
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.FullHDImageURL ?? src.LargeImageURL))
                .ForMember(des => des.ImageUri, opt => opt.MapFrom(src => src.FullHDImageURL ?? src.LargeImageURL))
                .ForMember(dest => dest.ImageAuthor, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.ImageAuthorUrl, opt => opt.MapFrom(src => src.UserImageURL))
                .ForMember(dest => dest.Background, opt => opt.MapFrom(src => CreateBackground()));

            profile.CreateMap<Photo, WallpaperDto>();
        }

        private AcrylicBrush CreateBackground()
        {
            var random = new Random();
            var index = random.Next(0, colors.Length);
            return new AcrylicBrush
            {
                BackgroundSource = AcrylicBackgroundSource.Backdrop,
                TintColor = colors[index],
                FallbackColor = colors[index],
                TintOpacity = 1.0,
                TintLuminosityOpacity = 1.0,
            };
        }
    }
}
