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
    /// https://picsum.photos/
    /// </summary>
    public class PicsumClient : IBaseClient<PicsumItem>
    {
        private readonly HttpClient _client;

        public PicsumClient(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(typeof(PicsumClient).FullName);
        }
        public async Task<IEnumerable<PicsumItem>> GetItemsAsync(int page, int per_page = 10, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://picsum.photos/v2/list?page={page}&limit={per_page}";

            var items = new List<PicsumItem>();

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await _client.GetAsync(new Uri(url), cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var picsums = JsonConvert.DeserializeObject<IEnumerable<PicsumItem>>(json);
            items.AddRange(picsums);

            return items;
        }
    }
}
