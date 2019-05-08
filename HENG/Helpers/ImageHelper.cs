using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace HENG.Helpers
{
    public static class ImageHelper
    {
        public static async Task<BitmapImage> StorageFileToBitmapImage(StorageFile sf)
        {
            using (IRandomAccessStream fileStream = await sf.OpenAsync(FileAccessMode.Read))
            {
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                return bitmapImage;
            }
        }
    }
}
