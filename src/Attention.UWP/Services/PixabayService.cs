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
    /// </summary>
    public class PixabayService
    {
        private readonly PixabaySharpClient _client;
        private readonly Filter _filter;
        public Filter DefaultFiler => _filter;

        public PixabayService(string api_key, Filter filter = default)
        {
            if (string.IsNullOrWhiteSpace(api_key))
                throw new KeyNotFoundException();

            _client = new PixabaySharpClient(api_key);
            _filter = filter ?? new Filter() { Category = Category.Animals, Order= Order.Popular };
        }


        public bool RefreshFilter(Filter filter)
        {
            bool ans = _filter != filter;
            if (ans)
            {
                _filter.Order = filter.Order;
                _filter.Orientation = filter.Orientation;
                _filter.ImageType = filter.ImageType;
                _filter.Category = filter.Category;
                _filter.Query = filter.Query;
            }
            return ans;
        }


        public async Task<ImageResult> QueryImagesAsync( int page = 1, int per_page = 20)
        {
            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Page = page,
                PerPage = per_page,
                Order = _filter.Order,
                Orientation = _filter.Orientation,
                ImageType = _filter.ImageType,
                Category = _filter.Category,
                Query = _filter.Query
            };
            return await _client.QueryImagesAsync(qb);
        }
    }
}
