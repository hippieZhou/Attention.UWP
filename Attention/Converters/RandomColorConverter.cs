using System;
using System.Linq;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Attention.Converters
{
    public class RandomColorConverter: IValueConverter
    {
        private readonly static Color[] colors;
        private readonly static Random random;
        static RandomColorConverter()
        {
            //ThemeColors = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null));
            colors = typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static).Select(c => (Color)c.GetValue(null)).ToArray();
            random = new Random(DateTime.UtcNow.Second);
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return colors[random.Next(colors.Length)];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
