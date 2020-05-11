using Attention.Core.Mappings;
using AutoMapper;
using PixabaySharp.Models;
using System;
using System.Linq;
using Unsplasharp.Models;
using Windows.UI.Xaml.Media;

namespace Attention.Core.Dtos
{
    public class WallpaperDto : BaseDto<WallpaperDto>, IMapFrom<ImageItem>, IMapFrom<Photo>
    {
        static WallpaperDto()
        {
            FakeData = colors.Select(x => new WallpaperDto
            {
                Background =CreateAcrylicBrush(x),
                ImageAuthor = DateTime.Now.Year.ToString(),
                ImageAuthorUrl = "https://www.bing.com",
                ImageUri = "ms-appx:///Assets/Images/bantersnaps-wPMvPMD9KBI-unsplash.jpg",
                Thumbnail = "ms-appx:///Assets/Images/bantersnaps-wPMvPMD9KBI-unsplash.jpg"
            });
        }

        public Brush Background { get; set; }
        public string Thumbnail { get; set; }
        public string ImageUri { get; set; }
        public string ImageAuthor { get; set; }
        public string ImageAuthorUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ImageItem, WallpaperDto>()
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.FullHDImageURL ?? src.LargeImageURL))
                .ForMember(des => des.ImageUri, opt => opt.MapFrom(src => src.FullHDImageURL ?? src.LargeImageURL))
                .ForMember(dest => dest.ImageAuthor, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.ImageAuthorUrl, opt => opt.MapFrom(src => src.UserImageURL))
                .ForMember(dest => dest.Background, opt => opt.MapFrom(src => CreateBackground()));

            profile.CreateMap<Photo, WallpaperDto>();
        }

        private Brush CreateBackground()
        {
            var random = new Random();
            var index = random.Next(0, colors.Length);
            return CreateAcrylicBrush(colors[index]);
        }
    }
}
