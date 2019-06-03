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
        private readonly PixabaySharpClient _client;
        public PixabayService(string apiKey)
        {
            _client = new PixabaySharpClient(apiKey);
        }

        public async Task<ImageResult> QueryImagesAsync(string query = "all", int page = 1, int per_page = 20)
        {
            ImageQueryBuilder qb = new ImageQueryBuilder()
            {
                Category = Category.Backgrounds,
                ImageType = ImageType.Photo,
                Page = page,
                PerPage = per_page
            };
            var result = await _client.QueryImagesAsync(qb);
            return result;
        }

        public Task<VideoResult> QueryVideosAsync(VideoQueryBuilder qb)
        {
            throw new NotImplementedException();
        }
    }
}
