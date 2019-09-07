using Attention.Core.Data;
using PixabaySharp;
using PixabaySharp.Enums;
using PixabaySharp.Models;
using PixabaySharp.Utility;
using System.Threading.Tasks;

namespace Attention.Core.Services
{
    public class PixabayService
    {
        private readonly PixabaySharpClient _client;

        public PixabayService(string api_key = "3153915-c1b347f3736d73ef2cd6a0e79")
        {
            _client = new PixabaySharpClient(api_key);
        }

        public async Task<ImageItemCollection> GetLatestPixabaies()
        {
            int page = 1;
            int per_page = 5;
            ImageResult result = await QueryImagesAsync(Order.Latest, Orientation.All, ImageType.All, page: page, per_page: per_page);
            return new ImageItemCollection(result.Images);
        }

        public async Task<ImageResult> QueryImagesAsync(Order order = default, Orientation orientation = default, ImageType imageType = default, Category category = default, int page = 1, int per_page = 20)
        {
            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Order = order,
                Orientation = orientation,
                ImageType = imageType,
                Category = category,
                Page = page,
                PerPage = per_page
            };
            return await _client.QueryImagesAsync(qb);
        }
    }
}
