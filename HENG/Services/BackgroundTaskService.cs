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
        public static async Task CacheImageAsync(object model, CancellationTokenSource cts, Action<IStorageFile, Exception> action)
        {
            string str = string.Empty;
            if (typeof(BingItem) == model.GetType())
            {
                str = (model as BingItem)?.Url;
            }
            else if (typeof(PaperItem) == model.GetType())
            {
                str = (model as PaperItem)?.Urls.Full;
            }

            if (!string.IsNullOrWhiteSpace(str))
            {
                var url = new Uri(str);
                var task = BackgroundDownloadHelper.Download(url, action);
                await task.ContinueWith(async (state) =>
                 {
                     if (state.Result == DownloadStartResult.AllreadyDownloaded)
                     {
                         var sf = await BackgroundDownloadHelper.CheckLocalFileExistsFromUriHash(url);
                         action(sf, null);
                     }
                 });
                System.Diagnostics.Debug.WriteLine("Downloading ...");
            }
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
