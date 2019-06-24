using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace HENG.App.Converters
{
    public class ImplicitDataTemplateConverter : IValueConverter
    {
        private readonly Dictionary<string, object> cached;
        public ImplicitDataTemplateConverter()
        {
            cached = new Dictionary<string, object>();
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && Application.Current != null)
            {
                string key = value.GetType().Name;
                if (!cached.ContainsKey(key))
                {
                    Application.Current.Resources.TryGetValue(value.GetType().Name, out object cache);
                    cached.Add(key, cache);
                }
                return cached.TryGetValue(key, out object dataTemplate) ? dataTemplate : null;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
