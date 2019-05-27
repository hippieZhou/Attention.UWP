using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HENG.Clients;
using HENG.Models;
using HENG.ViewModels;
using Microsoft.Toolkit.Uwp.UI;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using HENG.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using System.Linq;
using Windows.System.UserProfile;
using Windows.ApplicationModel.DataTransfer;
using HENG.Models.Shares;
using Windows.Storage.Search;
using GalaSoft.MvvmLight.Messaging;

namespace HENG.Services
{
    public partial class DataService
    {
        private IBaseClient<BingItem> Home_Client => ViewModelLocator.Current.ServiceProvider.GetService(typeof(BingClient)) as IBaseClient<BingItem>;
        private IBaseClient<PicsumItem> Picsum_Client => ViewModelLocator.Current.ServiceProvider.GetService(typeof(PicsumClient)) as IBaseClient<PicsumItem>;
        private PaperClient Paper_Client => ViewModelLocator.Current.ServiceProvider.GetService(typeof(PaperClient)) as PaperClient;

        public async Task<IEnumerable<BingItem>> GetItemsForBingAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var items = await Home_Client?.GetItemsAsync(++pageIndex, pageSize, cancellationToken);
            IEnumerable<string> urls = from p in items.Take(5) select p.Url;

            if (pageIndex == 1)
            {
                await Task.Run(() => { UpdateLiveTile(urls); });
            }

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

    public partial class DataService
    {
        public readonly List<DownloadItem> Downloads = new List<DownloadItem>();
        public async Task LoadHistoryAsync()
        {
            Downloads.Clear();

            StorageFolder downloadFolder = await StorageFolder.GetFolderFromPathAsync(App.Settings.DownloadPath);
            var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string> { ".jpg", ".png" });
            IReadOnlyList<StorageFile> sfs = await downloadFolder.CreateFileQueryWithOptions(queryOptions).GetFilesAsync();
            foreach (var sf in sfs)
            {
                BitmapImage photo = await ImageHelper.StorageFileToBitmapImage(sf);
                var item = new DownloadItem() { ResultFile = sf, Photo = photo };
                if (!Downloads.Contains(item))
                {
                    Downloads.Add(item);
                }
            }
        }

        public async Task DownLoad(object model)
        {
            if (model is BingItem t1)
            {
                await DownLoad(new Uri(t1.Url));
            }
            else if (model is PicsumItem t2)
            {
                await DownLoad(new Uri(t2.Download_url));
            }
            else if (model is PaperItem t3)
            {
                await DownLoad(new Uri(t3.Urls.Full));
            }
        }

        public async Task DownLoad(Uri sourceUri)
        {
            var download = new DownloadItem(sourceUri);
            await download.DownloadAsync();

            if (!Downloads.Contains(download))
            {
                Downloads.Add(download);
                Messenger.Default.Send(download);
            }
        }

        private void UpdateLiveTile(IEnumerable<string> urls)
        {
            var photosContent = new TileBindingContentPhotos();
            foreach (var item in urls)
            {
                photosContent.Images.Add(new TileBasicImage { Source = item, AddImageQuery = true });
            }

            var title = new TileBinding
            {
                Content = photosContent
            };
            var visual = new TileVisual
            {
                Branding = TileBranding.NameAndLogo,
                TileMedium = title,
                TileWide = title,
                TileLarge = title
            };

            var tileContent = new TileContent
            {
                Visual = visual
            };

            try
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(tileContent.GetXml()));
            }
            catch (Exception)
            {
            }
        }

        public async Task SetBackgroundAsync(string fileName)
        {
            if (UserProfilePersonalizationSettings.IsSupported())
            {
                var folder =await StorageFolder.GetFolderFromPathAsync(App.Settings.DownloadPath);
                var file =await folder.GetFileAsync(fileName);
                if (file != null)
                {
                    UserProfilePersonalizationSettings settings = UserProfilePersonalizationSettings.Current;
                    if (await settings.TrySetWallpaperImageAsync(file))
                    {

                    }
                }
            }
        }
    }

    public partial class DataService
    {
        public void Share(DownloadItem download)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += (sender, e) =>
            {
                var deferral = e.Request.GetDeferral();
                sender.TargetApplicationChosen += (s1, s2) =>
                {
                    deferral.Complete();
                };

                var data = new ShareSourceData("AppDisplayName".GetLocalized());
                if (download.RequestedUri != null)
                {
                    data.SetWebLink(download.RequestedUri);
                }
                if (download.ResultFile != null)
                {
                    data.SetImage(download.ResultFile as StorageFile);
                }

                if (data != null)
                {
                    e.Request.SetData(data);
                }
                e.Request.Data.OperationCompleted += (s, _) => { };
            };
            DataTransferManager.ShowShareUI();
        }
    }
}
