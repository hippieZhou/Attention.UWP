using Attention.UWP.Models;
using PixabaySharp;
using PixabaySharp.Enums;
using PixabaySharp.Models;
using PixabaySharp.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Attention.UWP.Services
{
    /// <summary>
    /// https://pixabay.com/api/docs/
    /// My:12645414-59a5251905dfea7b916dd796f
    /// </summary>
    public class PixabayService
    {
        private readonly PixabaySharpClient _client;
        private Filter _cacheFilter = new Filter()
        {
            Query = string.Empty,
            Order = Order.Latest,
            Orientation = Orientation.All,
            ImageType = ImageType.All,
            Category = Category.Backgrounds
        };

        public PixabayService(string api_key)
        {
            if (string.IsNullOrWhiteSpace(api_key))
            {
                var ex = new KeyNotFoundException("The API-KEY is missing");
                throw ex;
            }

            _client = new PixabaySharpClient(api_key);
        }

        private async Task<ImageResult> QueryImagesAsync(int page = 1, int per_page = 20)
        {
            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Page = page,
                PerPage = per_page,

                IsEditorsChoice = true,
                IsSafeSearch = true,
                ResponseGroup = ResponseGroup.HighResolution,

                Order = _cacheFilter.Order,
                Orientation = _cacheFilter.Orientation,
                ImageType = _cacheFilter.ImageType,
                Category = _cacheFilter.Category,
                Query = _cacheFilter.Query
            };
            return await _client.QueryImagesAsync(qb);
        }

        public async Task<ImageResult> QueryImagesAsync(int page = 1, int per_page = 20, Filter filter = default)
        {
            if (filter != null)
                _cacheFilter = filter;
            return await QueryImagesAsync(page, per_page);
        }
    }
}
