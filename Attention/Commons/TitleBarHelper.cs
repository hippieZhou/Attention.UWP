using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Attention.Commons
{
    public class TitleBarHelper
    {
        private static readonly TitleBarHelper _instance = new TitleBarHelper();
        public static TitleBarHelper Instance => _instance;

        private TitleBarHelper()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
        }

        public void RefreshTitleBar()
        {
            if (!ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
                return;

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (titleBar != null)
            {
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                if (ActualTheme() == ElementTheme.Dark)
                {
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.ForegroundColor = Colors.White;
                }
                else
                {
                    titleBar.ButtonForegroundColor = Colors.Black;
                    titleBar.ForegroundColor = Colors.Black;
                }

                titleBar.BackgroundColor = Colors.Black;

                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveForegroundColor = Colors.LightGray;
            }
        }

        private ElementTheme ActualTheme()
        {
            var frameworkElement = Window.Current.Content as FrameworkElement;
            return frameworkElement.ActualTheme;
        }
    }
}
