using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Attention.App.Converters
{
    public class ImageSourceNullValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var uri = value?.ToString()?.Trim();
            if (string.IsNullOrEmpty(uri))
            {
                if (string.IsNullOrEmpty(parameter?.ToString()?.Trim()))
                    throw new ArgumentNullException("Default ImageSource is Null.");
                uri = parameter.ToString();
            }

            return new BitmapImage(new Uri(uri));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
