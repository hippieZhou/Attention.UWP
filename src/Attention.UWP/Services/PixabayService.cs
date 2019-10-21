using PixabaySharp;
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
        public PixabayService(string api_key)
        {
            if (string.IsNullOrWhiteSpace(api_key))
                throw new KeyNotFoundException();

            _client = new PixabaySharpClient(api_key);
        }

        public async Task<ImageResult> QueryImagesAsync(int page = 1, int per_page = 20)
        {
            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Page = page,
                PerPage = per_page
            };
            return await _client.QueryImagesAsync(qb);
        }
    }
}
