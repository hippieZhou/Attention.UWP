using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HENG.Models;
using Newtonsoft.Json;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace HENG.Services
{
    /// <summary>
    /// https://github.com/shengqiangzhang/examples-of-web-crawlers
    /// </summary>
    public class DataService : IDataService
    {
        //static DataService()
        //{
        //    ImageCache.Instance.MaxMemoryCacheCount = 200;
        //    ImageCache.Instance.CacheDuration = TimeSpan.FromHours(24);
        //}

        public async Task<IEnumerable<BingItem>> GetBingsAsync(int page, int per_page = 10, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://hippiezhou.fun/api/bings?page={page}&per_page={per_page}";
            string json = await GetJsonAsync(url, cancellationToken, true);
            var data = JsonConvert.DeserializeObject<BingSource>(json);
            return data?.Bings;
        }

        public async Task<IEnumerable<PaperItem>> GetNewestAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c68ffb9463b7fbfe72b0db0?page={page}&per_page={per_page}";
            string json = await GetJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetHottestAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c69251c9b1c011c41bb97be?page={page}&per_page={per_page}";
            string json = await GetJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetGirlsAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c81087e6aee28c541eefc26?page={page}&per_page={per_page}";
            string json = await GetJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetSkyAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c81f64c96fad8fe211f5367?page={page}&per_page={per_page}";
            string json = await GetJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        public async Task<bool> DownloadImageAsync<T>(T model)
        {
            string coverUrl = string.Empty;
            if (model is BingItem bing)
            {
                coverUrl = bing.Url;
            }
            else if (model is PaperItem paper)
            {
                coverUrl = paper.Urls.Raw;
            }

            bool saved = false;
            if (!string.IsNullOrWhiteSpace(coverUrl))
            {
                try
                {
                    Uri source = new Uri(coverUrl);
                    StorageFile destinationFile = await KnownFolders.PicturesLibrary.CreateFileAsync($"aaa.jpg", CreationCollisionOption.GenerateUniqueName);
                    BackgroundDownloader downloader = new BackgroundDownloader();
                    DownloadOperation download = downloader.CreateDownload(source, destinationFile);
                    download.RangesDownloaded += (sender, e) =>
                     {

                     };
                    await download.StartAsync();
                }
                catch
                {
                    saved = false;
                }
            }
            return saved;
        }

        private async Task<string> GetJsonAsync(string url, CancellationToken token, bool bing = false)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    string json = string.Empty;

                    if (!token.IsCancellationRequested)
                    {
                        HttpResponseMessage response = null;
                        if (bing)
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("dev", "hippiezhou.fun");
                            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url)
                            {
                                Content = new StringContent("hippiezhou.fun", Encoding.UTF8, "application/json")
                            };
                            response = await client.SendAsync(request, token).ConfigureAwait(false);
                        }
                        else
                        {
                            response = await client.GetAsync(new Uri(url), token).ConfigureAwait(false);
                        }
                        response.EnsureSuccessStatusCode();
                        json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    return json;
                }
                catch (TaskCanceledException)
                {
                    return string.Empty;
                }
                catch (HttpRequestException)
                {
                    return string.Empty;
                }
            }
        }
    }
}
