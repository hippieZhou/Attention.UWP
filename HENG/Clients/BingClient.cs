using HENG.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HENG.Clients
{
    /// <summary>
    /// https://hippiezhou.fun
    /// </summary>
    public class BingClient: IBaseClient<BingItem>
    {
        private const string auth_str = "https://hippiezhou.fun/api/v1/auth/";
        private const string query_str = "https://hippiezhou.fun/api/v1/bing/";

        private readonly HttpClient _client;
        public string Token { get; private set; }

        public BingClient(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(typeof(BingClient).FullName);
        }

        public async Task<string> GetTokenAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(auth_str).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            JObject model = JsonConvert.DeserializeObject<JObject>(json);
            var token = model != null ? model["token"] : string.Empty;
            return token.ToString();
        }

        public async Task<IEnumerable<BingItem>> GetItemsAsync(int page, int per_page = 10, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = new List<BingItem>();

            if (string.IsNullOrWhiteSpace(Token))
            {
                Token = await GetTokenAsync();
            }

            var url = $"https://hippiezhou.fun/api/v1/bing/?page={page}&per_page={per_page}";
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", Token);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var bings = JsonConvert.DeserializeObject<BingSource>(json)?.Bings;
            items.AddRange(bings);

            return items;
        }
    }
}
