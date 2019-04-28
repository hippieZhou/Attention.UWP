using HENG.Helpers;
using HENG.Models;
using System.Threading;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;

namespace HENG.Services
{
    public class BackgroundTaskService
    {
        public static async Task CacheImageAsync(string address, Action<IStorageFile> action)
        {
            var url = new Uri(address);
            var task = BackgroundDownloadHelper.Download(url, async b =>
            {
                if (b)
                {
                    var sf = await BackgroundDownloadHelper.CheckLocalFileExistsFromUriHash(url);
                    action(sf);
                }
            });
            await task.ContinueWith(async (state) =>
            {
                if (state.Result == DownloadStartResult.AllreadyDownloaded)
                {
                    var sf = await BackgroundDownloadHelper.CheckLocalFileExistsFromUriHash(url);
                    action(sf);
                }
            });
        }

        public static async Task<BitmapImage> DrawImageAsync(IStorageFile file)
        {
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                try
                {
                    BitmapImage bitmapImage = new BitmapImage
                    {
                        DecodePixelHeight = 100,
                        DecodePixelWidth = 100
                    };
                    await bitmapImage.SetSourceAsync(fileStream);
                    return bitmapImage;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
