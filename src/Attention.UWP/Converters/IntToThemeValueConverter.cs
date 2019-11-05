using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Attention.UWP.Converters
{
    public class IntToThemeValueConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Enum.TryParse(value?.ToString(), out ElementTheme theme) ? theme : ElementTheme.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
