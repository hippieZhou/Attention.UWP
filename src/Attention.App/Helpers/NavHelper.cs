using System;
using Windows.UI.Xaml;

namespace Attention.App.Helpers
{
    public class NavHelper
    {
        public static string GetNavTo(DependencyObject obj)
        {
            return (string)obj.GetValue(NavToProperty);
        }

        public static void SetNavTo(DependencyObject obj, string value)
        {
            obj.SetValue(NavToProperty, value);
        }

        // Using a DependencyProperty as the backing store for NavTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavToProperty =
            DependencyProperty.RegisterAttached("NavTo", typeof(string), typeof(NavHelper), new PropertyMetadata(default));
    }
}
