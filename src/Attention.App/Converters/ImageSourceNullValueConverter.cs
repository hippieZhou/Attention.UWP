using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Attention.App.Converters
{
    public class ImageSourceNullValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var defaultSource = parameter?.ToString();
            if (value == null && string.IsNullOrWhiteSpace(defaultSource))
            {
                throw new ArgumentNullException("ImageSource is Null.");
            }

            if (value is BitmapImage bitmapImage)
            {
                return bitmapImage;
            }

            if (value is string wantSource && !string.IsNullOrWhiteSpace(wantSource))
            {
                new BitmapImage(new Uri(wantSource, UriKind.RelativeOrAbsolute));
            }

            return new BitmapImage(new Uri(defaultSource, UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
