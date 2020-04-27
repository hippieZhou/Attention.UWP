using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Attention.App.Converters
{
    public class ImageSourceNullValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is BitmapImage bitmapImage)
            {
                return bitmapImage;
            }

            var uri = value is string passUri && !string.IsNullOrWhiteSpace(passUri) ? passUri : parameter?.ToString();
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException("ImageSource is Null.");
            }
            return new BitmapImage(new Uri(uri));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
