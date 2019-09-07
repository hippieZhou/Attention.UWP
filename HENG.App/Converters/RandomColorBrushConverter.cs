using HENG.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace HENG.App.Converters
{
    public class RandomColorBrushConverter : IValueConverter
    {
        public static IEnumerable<Color> ThemeColors => typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null));

        private readonly Random random = new Random(DateTime.UtcNow.Second);
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int index = random.Next(ThemeColors.Count() - 1);
            return ThemeColors.ElementAt(index);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
