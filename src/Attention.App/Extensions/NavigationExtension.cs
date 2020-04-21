using Prism.Windows.Navigation;
using Windows.UI.Xaml.Controls;

namespace Attention.App.Extensions
{
    public static class NavigationExtension
    {
        public static void NavigateToPage<T>(this INavigationService navService, object parameter) where T : Page
        {
            var pageToken = typeof(T).Name.Replace(nameof(Page), "");
            navService.Navigate(pageToken, parameter);
        }
    }
}
