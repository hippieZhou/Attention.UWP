using Attention.Models;
using PixabaySharp;
using PixabaySharp.Enums;
using PixabaySharp.Models;
using PixabaySharp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attention.Services
{
    /// <summary>
    /// https://pixabay.com/api/docs/
    /// </summary>
    public class PixabayService
    {
        private readonly PixabaySharpClient _client;
        public PixabayService(string api_key)
        {
            _client = new PixabaySharpClient(api_key);
        }

        public (IEnumerable<FilterItem> orders, IEnumerable<FilterItem> orientations, IEnumerable<FilterItem> imageTypes, IEnumerable<FilterItem> categories) GetEnumFilters()
        {
            IEnumerable<FilterItem> GetDefinedValues<T>(Type type)
            {
                IEnumerable<T> items = from p in Enum.GetValues(type).Cast<T>() select p;
                return from p in items select new FilterItem { Name = p.ToString(), Checked = false };
            }

            IEnumerable<FilterItem> t1 = GetDefinedValues<Order>(typeof(Order));
            IEnumerable<FilterItem> t2 = GetDefinedValues<Orientation>(typeof(Orientation));
            IEnumerable<FilterItem> t3 = GetDefinedValues<ImageType>(typeof(ImageType));
            IEnumerable<FilterItem> t4 = GetDefinedValues<Category>(typeof(Category));
            return (t1, t2, t3, t4);
        }

        public async Task<ImageResult> QueryImagesAsync((FilterItem Order, FilterItem Orientation, FilterItem ImageType, FilterItem Category) filter, int page = 1, int per_page = 20)
        {
            Enum.TryParse(filter.Order?.Name, out Order order);
            Enum.TryParse(filter.Orientation?.Name, out Orientation orientation);
            Enum.TryParse(filter.ImageType?.Name, out ImageType imageType);
            Enum.TryParse(filter.ImageType?.Name, out Category category);

            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Order = order,
                Orientation = orientation,
                ImageType = imageType,
                Category = category,
                Page = page,
                PerPage = per_page
            };
            var result = await _client.QueryImagesAsync(qb);
            return result;
        }
    }
}
