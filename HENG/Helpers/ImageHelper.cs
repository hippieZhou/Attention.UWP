using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace HENG.Helpers
{
    public static class ImageHelper
    {
        public static async Task<BitmapImage> StorageFileToBitmapImage(IStorageFile sf)
        {
            using (IRandomAccessStream stream = await sf.OpenAsync(FileAccessMode.Read))
            {
                var bitmap = new BitmapImage();
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmap.SetSource(stream);
                return bitmap;
            }
        }
    }
}
