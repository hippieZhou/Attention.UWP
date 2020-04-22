using Attention.App.Models;
using AutoMapper;
using PixabaySharp;
using PixabaySharp.Enums;
using PixabaySharp.Models;
using PixabaySharp.Utility;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.App.Services
{
    public class PixabayMappingProfile : Profile
    {
        public PixabayMappingProfile()
        {
            CreateMap<ImageItem, WallpaperEntity>();
        }
    }

    public class PixabayService : WallpaperService, IWallpaperService
    {
        private readonly PixabaySharpClient _client;

        public PixabayService(string apiKey) : base(apiKey) => _client = new PixabaySharpClient(APIKEY);

        public override async Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int page, int perPage, CancellationToken cancellationToken = default)
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
            return MapToEntities(imageResult.Images);
        }
    }
}
