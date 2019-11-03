using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Attention.UWP.Converters
{
    public class ImageItemColorConverter: IValueConverter
    {
        private readonly static IEnumerable<Color> colors;
        private readonly static Random random;
        static ImageItemColorConverter()
        {
            //colors = typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static).Select(c => (Color)c.GetValue(null)).ToArray();
            colors = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null));
            random = new Random(DateTime.UtcNow.Second);
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var first = colors.ElementAt(random.Next(colors.Count()));
            var brush = new AcrylicBrush
            {
                BackgroundSource = AcrylicBackgroundSource.Backdrop,
                FallbackColor = first,
                TintColor = first,
                TintOpacity = 0.4,
            };
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
