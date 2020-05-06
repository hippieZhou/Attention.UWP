using Attention.Core.Entities;
using Attention.Core.Framework;
using AutoMapper;
using PixabaySharp;
using PixabaySharp.Enums;
using PixabaySharp.Models;
using PixabaySharp.Utility;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Core.Services
{
    public class PixabayMappingProfile : Profile
    {
        public PixabayMappingProfile()
        {
            CreateMap<ImageItem, WallpaperEntity>();
        }
    }

    public class PixabayWebClient : IWebClient
    {
        private readonly PixabaySharpClient _client;

        public PixabayWebClient(string apiKey) => _client = new PixabaySharpClient(apiKey);

        public async Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int page, int perPage, CancellationToken cancellationToken = default)
        {
            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Page = page,
                PerPage = perPage,

                IsEditorsChoice = true,
                IsSafeSearch = true,
                ResponseGroup = ResponseGroup.HighResolution
            };

            var imageResult = await _client.QueryImagesAsync(qb);

            var mapper = EnginContext.Current.Resolve<IMapper>();
            return mapper.Map<IEnumerable<WallpaperEntity>>(imageResult?.Images);
        }
    }
}
