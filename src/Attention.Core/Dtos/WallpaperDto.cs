using Attention.Core.Extensions;
using Attention.Core.Mappings;
using Attention.Core.Services;
using AutoMapper;
using PixabaySharp.Models;
using Unsplasharp.Models;
using Windows.UI.Xaml.Media;

namespace Attention.Core.Dtos
{
    public class WallpaperDto : IMapFrom<ImageItem>, IMapFrom<Photo>
    {
        public Brush Background { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }
        public string ImageDownloadUri { get; set; }
        public string Author { get; set; }
        public string AuthorUri { get; set; }
        public string AuthorAvatar { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ImageItem, WallpaperDto>()
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.FullHDImageURL ?? src.LargeImageURL))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Tags))
                .ForMember(des => des.ImageUri, opt => opt.MapFrom(src => src.FullHDImageURL ?? src.LargeImageURL))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.AuthorUri, opt => opt.MapFrom(src => "https://www.bing.com"))
                .ForMember(dest => dest.AuthorAvatar, opt => opt.MapFrom(src => src.UserImageURL))
                .ForMember(dest => dest.Background, opt => opt.MapFrom(src => DesignDataService.Brushs.Random()));

            profile.CreateMap<Photo, WallpaperDto>();
        }
    }
}
