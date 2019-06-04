using PixabaySharp;
using PixabaySharp.Enums;
using PixabaySharp.Models;
using PixabaySharp.Utility;
using System;
using System.Threading.Tasks;

namespace HENG.Core.Services
{
    /// <summary>
    /// https://pixabay.com/api/docs/
    /// </summary>
    public class PixabayService
    {
        public string QueryText;

        private readonly PixabaySharpClient _client;
        public PixabayService(string apiKey= "12645414-59a5251905dfea7b916dd796f")
        {
            apiKey = "3153915-c1b347f3736d73ef2cd6a0e79";
            _client = new PixabaySharpClient(apiKey);
        }

        public async Task<ImageResult> QueryImagesAsync(int page = 1, int per_page = 20)
        {
            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Query = QueryText,
                Orientation = Orientation.All,
                Page = page,
                PerPage = per_page
            };
            var result = await _client.QueryImagesAsync(qb);
            return result;
        }
    }
}
