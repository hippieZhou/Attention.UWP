using Attention.App.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unsplasharp;
using Unsplasharp.Models;
using static Unsplasharp.UnsplasharpClient;

namespace Attention.App.Services
{
    public class UnsplashMappingProfile : Profile
    {
        public UnsplashMappingProfile()
        {
            CreateMap<Photo, WallpaperDto>();
        }
    }

    public class UnsplashService : WallpaperService, IWallpaperService
    {
        private readonly UnsplasharpClient _client;

        public UnsplashService(string apiKey) : base(apiKey) => _client = new UnsplasharpClient(APIKEY);

        public override async Task<IEnumerable<WallpaperDto>> GetPagedItemsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var listPhotos = await _client.ListPhotos(page: page, perPage: pageSize, orderBy: OrderBy.Popular);
            return MapToEntities(listPhotos);
        }
    }
}
