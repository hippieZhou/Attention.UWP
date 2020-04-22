using Attention.App.Models;
using PixabaySharp;
using PixabaySharp.Enums;
using PixabaySharp.Utility;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.App.Services
{
    public class PixabayService : WallpaperService, IWallpaperService
    {
        private readonly PixabaySharpClient _client;

        public PixabayService(string apiKey) : base(apiKey) => _client = new PixabaySharpClient(APIKEY);

        public override async Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Page = pageIndex,
                PerPage = pageSize,

                IsEditorsChoice = true,
                IsSafeSearch = true,
                ResponseGroup = ResponseGroup.HighResolution
            };

            var listPhotos = await _client.QueryImagesAsync(qb);

            return default;
        }
    }
}
