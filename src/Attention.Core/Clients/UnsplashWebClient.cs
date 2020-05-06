using Attention.Core.Entities;
using Attention.Core.Framework;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unsplasharp;
using Unsplasharp.Models;

namespace Attention.Core.Services
{
    public class UnsplashMappingProfile : Profile
    {
        public UnsplashMappingProfile()
        {
            CreateMap<Photo, WallpaperEntity>();
        }
    }

    public class UnsplashWebClient : IWebClient
    {
        private readonly UnsplasharpClient _client;

        public UnsplashWebClient(string apiKey, string secret) => _client = new UnsplasharpClient(apiKey, secret);

        public async Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int page, int perPage, CancellationToken cancellationToken = default)
        {
            var listPhotos = await _client.ListPhotos(page: page, perPage: perPage, orderBy: UnsplasharpClient.OrderBy.Popular);
            var mapper = EnginContext.Current.Resolve<IMapper>();
            return mapper.Map<IEnumerable<WallpaperEntity>>(listPhotos);
        }
    }
}
