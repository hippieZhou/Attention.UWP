using Attention.UWP.Models;
using MetroLog;
using PixabaySharp;
using PixabaySharp.Enums;
using PixabaySharp.Models;
using PixabaySharp.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Attention.UWP.Services
{
    /// <summary>
    /// https://pixabay.com/api/docs/
    /// </summary>
    public class PixabayService
    {
        private readonly ILogger _logger;

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
            _logger = ViewModels.ViewModelLocator.Current.LogManager.GetLogger<PixabayService>();

            if (string.IsNullOrWhiteSpace(api_key))
            {
                var ex = new KeyNotFoundException();
                _logger.Error("The API-KEY is missing", ex);
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
                Order = _cacheFilter.Order,
                Orientation = _cacheFilter.Orientation,
                ImageType = _cacheFilter.ImageType,
                Category = _cacheFilter.Category,
                Query = _cacheFilter.Query
            };
            try
            {
                return await _client.QueryImagesAsync(qb);
            }
            catch (Exception ex)
            {
                _logger.Error("QueryImagesAsync:", ex);
                return default(ImageResult);
            }
        }

        public async Task<ImageResult> QueryImagesAsync(int page = 1, int per_page = 20, Filter filter = default)
        {
            if (filter != null)
                _cacheFilter = filter;
            return await QueryImagesAsync(page, per_page);
        }
    }
}
