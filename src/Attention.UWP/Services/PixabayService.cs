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
        public PixabayService(string api_key, Filter filter)
        {
            if (string.IsNullOrWhiteSpace(api_key))
                throw new KeyNotFoundException();

            _client = new PixabaySharpClient(api_key);
            _filter = filter;
        }

        public async Task<ImageResult> QueryImagesAsync(int page = 1, int per_page = 20)
        {
            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Page = page,
                PerPage = per_page,
                Order = _filter.Order,
                Orientation = _filter.Orientation,
                ImageType = _filter.ImageType,
                Category = _filter.Category,
                Language = _filter.Language,
                Query = _filter.Query
            };
            return await _client.QueryImagesAsync(qb);
        }

        public void ChangeFiler(
            Order order = default, Orientation orientation = default,
            ImageType imageType = default, Category category = default,
            Language language = default, string query = default)
        {
            _filter.Order = order;
            _filter.Orientation = orientation;
            _filter.ImageType = imageType;
            _filter.Category = category;
            _filter.Language = language;
            _filter.Query = query;
        }
    }
}
