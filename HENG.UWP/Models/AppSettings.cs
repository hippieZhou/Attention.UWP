using GalaSoft.MvvmLight;
using HENG.UWP.Extensions;
using HENG.UWP.Helpers;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace HENG.UWP.Models
{
    public class AppSettings : ObservableObject
    {
        private ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;

        public AppSettings()
        {
            if (SystemInformation.IsFirstRun)
            {
                ThemeMode = (int)ElementTheme.Default;
                Language = Windows.Globalization.Language.CurrentInputMethodLanguageTag == "zh-Hans-CN" ? 0 : 1;
            }
        }

        public string AppName => "AppDisplayName".GetLocalized();

        public string AppVersion
        {
            get
            {
                var packageVersion = Package.Current.Id.Version;
                var version = $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";
                return version;
            }
        }

        public async Task<string> GetDownloadPathAsync()
        {
            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("HENG", CreationCollisionOption.OpenIfExists);
            return folder.Path;
        }

        public int ThemeMode
        {
            get { return GetSettingsValue(nameof(ThemeMode), (int)ElementTheme.Default); ; }
            set
            {
                SetSettingsValue(nameof(ThemeMode), value);
                RaisePropertyChanged(() => ThemeMode);

                UpdateTheme();
            }
        }

        public int Language
        {
            get { return GetSettingsValue(nameof(Language), 0); }
            set
            {
                SetSettingsValue(nameof(Language), value);
                RaisePropertyChanged(() => Language);

                UpdateLanguage();
            }
        }

        public bool EnableLiveTitle
        {
            get { return GetSettingsValue(nameof(EnableLiveTitle), true); }
            set
            {
                SetSettingsValue(nameof(EnableLiveTitle), value);
                RaisePropertyChanged(() => EnableLiveTitle);

                UpdateLiveTitle();
            }
        }

        private void UpdateTheme()
        {
            var theme = (ElementTheme)ThemeMode;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonForegroundColor = theme == ElementTheme.Default || theme == ElementTheme.Light ? Colors.Black : Colors.White;
            if (Window.Current.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = theme;
            }
        }

        private void UpdateLanguage()
        {
            ApplicationLanguages.PrimaryLanguageOverride = Language == 0 ? "zh-Hans-CN" : "en-US";
        }

        private void UpdateLiveTitle()
        {
            if (EnableLiveTitle)
            {
                LiveTileUpdater.UpdateLiveTile();
            }
            else
            {
                LiveTileUpdater.CleanUpTile();
            }
        }

        private TResult GetSettingsValue<TResult>(string name, TResult defaultValue)
        {
            try
            {
                if (!LocalSettings.Values.ContainsKey(name))
                {
                    LocalSettings.Values[name] = defaultValue;
                }
                return (TResult)LocalSettings.Values[name];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return defaultValue;
            }
        }

        private void SetSettingsValue(string name, object value)
        {
            LocalSettings.Values[name] = value;
        }
    }
}
