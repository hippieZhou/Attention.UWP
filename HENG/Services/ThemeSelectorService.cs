using HENG.Helpers;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace HENG.Services
{
    public static class ThemeSelectorService
    {
        private const string SettingsKey = "AppBackgroundRequestedTheme";

        public static ElementTheme Theme { get; set; } = ElementTheme.Default;

        static ThemeSelectorService()
        {
            var Listener = new ThemeListener();
            Listener.ThemeChanged += (sender) =>
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonForegroundColor = sender.CurrentTheme == ApplicationTheme.Light ? Colors.Black : Colors.White;
            };
        }

        public static async Task InitializeAsync()
        {
            Theme = await LoadThemeFromSettingsAsync();

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonForegroundColor = Theme == ElementTheme.Default || Theme == ElementTheme.Light ? Colors.Black : Colors.White;
        }

        public static async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonForegroundColor = Theme == ElementTheme.Light || Theme == ElementTheme.Default ? Colors.Black : Colors.White;

            await SetRequestedThemeAsync();
            await SaveThemeInSettingsAsync(Theme);
        }

        public static async Task SetRequestedThemeAsync()
        {
            foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        frameworkElement.RequestedTheme = Theme;
                    }
                });
            }
        }

        private static async Task<ElementTheme> LoadThemeFromSettingsAsync()
        {
            ElementTheme cacheTheme = ElementTheme.Default;
            string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);

            if (!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out cacheTheme);
            }

            return cacheTheme;
        }

        private static async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString());
        }
    }
}
