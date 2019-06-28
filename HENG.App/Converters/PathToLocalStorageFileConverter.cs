using HENG.App.Models;
using HENG.App.Services;
using System;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace HENG.App.Converters
{
    public class PathToLocalStorageFileConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return value;
            }

            string fileName = DownloadService.GetFileNameFromUri(new Uri(value.ToString()));
            IStorageItem file = StorageFolder.GetFolderFromPathAsync(AppSettings.Current.DownloadPath).AsTask().Result.TryGetItemAsync(fileName).AsTask().Result;
            if (file == null)
            {
                return null;
            }

            var stream = (file as StorageFile).OpenReadAsync().AsTask().Result;
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
