using HENG.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Clear();
            query.Add("page", page.ToString());
            query.Add("per_page", per_page.ToString());

            var builder = new UriBuilder(query_str)
            {
                Query = query.ToString()
            };

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", Token);
            HttpResponseMessage response = await _client.GetAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var bings = JsonConvert.DeserializeObject<BingSource>(json)?.Bings;
            items.AddRange(bings);

            return items;
        }
    }
}
