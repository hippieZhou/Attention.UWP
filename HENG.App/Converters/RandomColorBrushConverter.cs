using HENG.App.Services;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace HENG.App.Converters
{
    public class RandomColorBrushConverter : IValueConverter
    {
        private readonly Random random = new Random(DateTime.UtcNow.Second);
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int index = random.Next(DataService.SystemColors.Count() - 1);
            return DataService.SystemColors.ElementAt(index);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
