using HENG.UWP.Models;
using PixabaySharp;
using PixabaySharp.Enums;
using PixabaySharp.Models;
using PixabaySharp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HENG.UWP.Services
{
    /// <summary>
    /// https://pixabay.com/api/docs/
    /// </summary>
    public class PixabayService
    {
        public string QueryText;

        private readonly PixabaySharpClient _client;
        public PixabayService(string apiKey)
        {
            _client = new PixabaySharpClient(apiKey);
        }

        public IEnumerable<ParameterModel> GetOptionalParameters()
        {
            IEnumerable<OptionalParameter> Parse<T>(Type type)
            {
                IEnumerable<T> items = from p in Enum.GetValues(type).Cast<T>() select p;
                return from p in items select new OptionalParameter { Description = p.ToString(), Checked = false };
            }

            List<ParameterModel> list = new List<ParameterModel>();

            IEnumerable<OptionalParameter> t1 = Parse<ImageType>(typeof(ImageType));
            list.Add(new ParameterModel() { Name = nameof(ImageType), Items = t1 });

            IEnumerable<OptionalParameter> t2 = Parse<Orientation>(typeof(Orientation));
            list.Add(new ParameterModel() { Name = nameof(Orientation), Items = t2 });

            IEnumerable<OptionalParameter> t3 = Parse<Category>(typeof(Category));
            list.Add(new ParameterModel() { Name = nameof(Category), Items = t3 });

            IEnumerable<OptionalParameter> t4 = Parse<Order>(typeof(Order));
            list.Add(new ParameterModel() { Name = nameof(Order), Items = t4 });

            return list;
        }

        public async Task<ImageResult> QueryImagesAsync(int page = 1, int per_page = 20)
        {
            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Orientation = Orientation.All,
                Page = page,
                PerPage = per_page
            };
            var result = await _client.QueryImagesAsync(qb);
            return result;
        }
    }
}
