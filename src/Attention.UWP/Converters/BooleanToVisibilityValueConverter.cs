using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Attention.UWP.Converters
{
    public class BooleanToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool.TryParse(value?.ToString(), out bool isChecked);
            bool.TryParse(parameter?.ToString(), out bool reversed);
            return isChecked == reversed ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
