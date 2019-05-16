using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HENG.Clients;
using HENG.Models;
using HENG.ViewModels;
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
    public partial class DataService
    {
        private IBaseClient<BingItem> Home_Client => ViewModelLocator.Current.ServiceProvider.GetService(typeof(BingClient)) as IBaseClient<BingItem>;
        private IBaseClient<PicsumItem> Picsum_Client => ViewModelLocator.Current.ServiceProvider.GetService(typeof(PicsumClient)) as IBaseClient<PicsumItem>;
        private PaperClient Paper_Client => ViewModelLocator.Current.ServiceProvider.GetService(typeof(PaperClient)) as PaperClient;

        public async Task<IEnumerable<BingItem>> GetItemsForBingAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Home_Client?.GetItemsAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }

        public async Task<IEnumerable<PicsumItem>> GetItemsForPicsumAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Picsum_Client?.GetItemsAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetItemsForNewestAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Paper_Client?.GetNewestAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetItemsForHottestAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Paper_Client?.GetHottestAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetItemsForGirlsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Paper_Client?.GetGirlsAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetItemsForSkylandAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Paper_Client?.GetSkylandAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }
    }
}
