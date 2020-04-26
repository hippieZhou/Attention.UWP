using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Attention.App.Models
{
    public class AppSettings : BindableBase
    {
        private readonly ApplicationDataContainer _localSettings;

        public string Version
        {
            get
            {
                PackageVersion packageVersion = Package.Current.Id.Version;
                return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";
            }
        }

        private ElementTheme _theme;
        public ElementTheme Theme
        {
            get { return _theme; }
            set
            {
                if (_theme != value)
                {
                    SetProperty(ref _theme, value);
                    SaveSettings(nameof(Theme), (int)Theme);
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        frameworkElement.RequestedTheme = Theme;
                    }
                }
            }
        }

        private string _language;
        public string Language
        {
            get { return _language; }
            set
            {
                if (_language != value)
                {
                    SetProperty(ref _language, value);
                    SaveSettings(nameof(Language), Language);
                    ApplicationLanguages.PrimaryLanguageOverride = Language;
                }
            }
        }

        public AppSettings()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
            LoadSettings();
        }

        private void LoadSettings()
        {
            Theme = (ElementTheme)Enum.ToObject(typeof(ElementTheme), ReadSettings(nameof(Theme), (int)ElementTheme.Default));
            var lang = ApplicationLanguages.PrimaryLanguageOverride;
            if (string.IsNullOrWhiteSpace(lang))
            {
                lang = ApplicationLanguages.Languages[0];
            }
            Language = lang.StartsWith("zh") ? "zh-CN" : "en-US";
        }

        public async Task<StorageFolder> GetSavedFolderAsync() => await KnownFolders.PicturesLibrary.CreateFolderAsync("Attention", CreationCollisionOption.OpenIfExists);

        public async Task<StorageFolder> GetTemporaryFolderAsync() => await ApplicationData.Current.TemporaryFolder.CreateFolderAsync("ImageCache", CreationCollisionOption.OpenIfExists);


        private void SaveSettings(string key, object value)
        {
            _localSettings.Values[key] = value;
        }
        private T ReadSettings<T>(string key, T defaultValue)
        {
            if (_localSettings.Values.ContainsKey(key))
            {
                return (T)_localSettings.Values[key];
            }
            if (defaultValue != null)
            {
                return defaultValue;
            }
            return default;
        }
    }
}
