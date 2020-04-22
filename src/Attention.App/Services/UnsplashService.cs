using Attention.App.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unsplasharp;
using static Unsplasharp.UnsplasharpClient;

namespace Attention.App.Services
{
    public class UnsplashService : WallpaperService, IWallpaperService
    {
        private readonly UnsplasharpClient _client;

        public UnsplashService(string apiKey) : base(apiKey) => _client = new UnsplasharpClient(APIKEY);

        public override async Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var listPhotos = await _client.ListPhotos(page: pageIndex, perPage: pageSize, orderBy: OrderBy.Popular);
            return default;
        }
    }
}
