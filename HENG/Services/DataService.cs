using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HENG.Models;
using Microsoft.Toolkit.Uwp.UI;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace HENG.Services
{
    public partial class DataService
    {
        static DataService()
        {
            ImageCache.Instance.MaxMemoryCacheCount = 1000;
            ImageCache.Instance.CacheDuration = TimeSpan.FromHours(24);
        }

        public async Task GetFromCacheAsync(string url, Action<BitmapImage> action)
        {
            var file = await ImageCache.Instance.GetFileFromCacheAsync(new Uri(url));
            if (file != null)
            {
                var props = await file.GetBasicPropertiesAsync();
                if (props.Size == 0)
                {
                    await file.DeleteAsync();
                }
            }
            var task = ImageCache.Instance.PreCacheAsync(new Uri(url));
            await task.ContinueWith(async t =>
            {
                var bmp = await ImageCache.Instance.GetFromCacheAsync(new Uri(url));
                action(bmp);
            }).ConfigureAwait(false); ;
        }

        public async Task<StorageFile> GetFileFromCacheAsync(string url)
        {
            var sf = await ImageCache.Instance.GetFileFromCacheAsync(new Uri(url));
            return sf;
        }
    }
    /// <summary>
    /// https://hippiezhou.fun
    /// </summary>
    public partial class DataService
    {
        public async Task<IEnumerable<BingItem>> GetBingsAsync(int page, int per_page = 10, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://hippiezhou.fun/api/bings?page={page}&per_page={per_page}";
            //var url = $"https://hippiezhou.fun/v1/bing/all?page={page}&per_page={per_page}";
            string json = await GetBingJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<BingSource>(json)?.Bings;
            return items;
        }

        private async Task<string> GetBingJsonAsync(string url, CancellationToken token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = string.Empty;

                if (!token.IsCancellationRequested)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("dev", "hippiezhou.fun");
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url)
                    {
                        Content = new StringContent("hippiezhou.fun", Encoding.UTF8, "application/json")
                    };
                    HttpResponseMessage response = await client.SendAsync(request, token).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                return json;
            }
        }
    }

    /// <summary>
    /// https://github.com/shengqiangzhang/examples-of-web-crawlers
    /// http://paper.meiyuan.in/
    /// </summary>
    public partial class DataService
    {
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

        public async Task<IEnumerable<PaperItem>> GetSkyAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
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
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
    }

    /// <summary>
    /// https://picsum.photos/
    /// </summary>
    public partial class DataService
    {
        public async Task<IEnumerable<PicsumItem>> GetPicsumAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://picsum.photos/v2/list?page={page}&limit={per_page}";
            string json = await GetPicsumJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PicsumItem>>(json);
            return items;
        }

        private async Task<string> GetPicsumJsonAsync(string url, CancellationToken token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
    }
}
