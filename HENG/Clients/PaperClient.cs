using HENG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace HENG.Clients
{
    /// <summary>
    /// http://paper.meiyuan.in/
    /// https://github.com/shengqiangzhang/examples-of-web-crawlers
    /// </summary>
    public class PaperClient : IBaseClient<PaperItem>
    {
        private readonly HttpClient _client;

        public PaperClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<PaperItem>> GetNewestAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c68ffb9463b7fbfe72b0db0?page={page}&per_page={per_page}";
            string json = await GetPaperJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetHottestAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c69251c9b1c011c41bb97be?page={page}&per_page={per_page}";
            string json = await GetPaperJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetGirlsAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c81087e6aee28c541eefc26?page={page}&per_page={per_page}";
            string json = await GetPaperJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetSkylandAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c81f64c96fad8fe211f5367?page={page}&per_page={per_page}";
            string json = await GetPaperJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        private async Task<string> GetPaperJsonAsync(string url, CancellationToken token)
        {
            using (var client = new HttpClient())
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = string.Empty;

                if (!token.IsCancellationRequested)
                {
                    HttpResponseMessage response = await client.GetAsync(new Uri(url), token).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                return json;
            }
        }


        public Task<IEnumerable<PaperItem>> GetItemsAsync(int page, int per_page = 10, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
