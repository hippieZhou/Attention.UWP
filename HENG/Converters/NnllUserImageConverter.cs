using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace HENG.Converters
{
    public class NnllUserImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var url = value?.ToString();
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    return new BitmapImage(new Uri(url));
                }
                catch (Exception)
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/UserPlaceholder.png"));
                }
            }
            else
            {
                return new BitmapImage(new Uri("ms-appx:///Assets/UserPlaceholder.png"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
