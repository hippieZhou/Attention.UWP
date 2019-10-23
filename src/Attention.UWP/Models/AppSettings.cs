using Attention.UWP.Extensions;
using Attention.UWP.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Attention.UWP.Models
{
    public class AppSettings : ObservableObject
    {
        private readonly ApplicationDataContainer localSettings;
        public string Name => "AppDisplayName".GetLocalized();
        public string Version
        {
            get
            {
                PackageVersion packageVersion = Package.Current.Id.Version;
                return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";
            }
        }

        private const int ELEMENTTHEME_DEFAULT = 0;
        private const int ELEMENTTHEME_DARK = 2;
        public int Theme
        {
            get { return ReadSettings(nameof(Theme), ELEMENTTHEME_DEFAULT); }
            set
            {
                if (value < ELEMENTTHEME_DEFAULT) value = ELEMENTTHEME_DEFAULT;
                if (value > ELEMENTTHEME_DARK) value = ELEMENTTHEME_DARK;

                SaveSettings(nameof(Theme), value);
                RaisePropertyChanged(() => Theme);

                if (Window.Current.Content is FrameworkElement rootElement)
                {
                    var theme = (ElementTheme)Enum.ToObject(typeof(ElementTheme), Theme);
                    rootElement.RequestedTheme = theme;
                    Messenger.Default.Send(theme, nameof(ExtendedTitleBar));
                }
            }
        }

        private const int LANGUAGE_ZH = 0;
        private const int LANGUAGE_EN = 1;
        public int Language
        {
            get { return ReadSettings(nameof(Language), LANGUAGE_ZH); }
            set
            {
                if (value < LANGUAGE_ZH) value = LANGUAGE_ZH;
                if (value > LANGUAGE_EN) value = LANGUAGE_EN;

                SaveSettings(nameof(Language), value);
                RaisePropertyChanged(() => Language);

                ApplicationLanguages.PrimaryLanguageOverride = value == LANGUAGE_ZH ? "zh-CN" : "en-US";
                Messenger.Default.Send("more_Personalized_Language_restart".GetLocalized(), nameof(InAppNotification));
            }
        }

        private const int SCROLLHEADERMODE_NONE = 0;
        private const int SCROLLHEADERMODE_FADE = 3;
        public int HeaderMode
        {
            get { return ReadSettings(nameof(HeaderMode), SCROLLHEADERMODE_NONE); }
            set
            {
                if (value < SCROLLHEADERMODE_NONE) value = SCROLLHEADERMODE_NONE;
                if (value > SCROLLHEADERMODE_FADE) value = SCROLLHEADERMODE_FADE;

                SaveSettings(nameof(HeaderMode), value);
                RaisePropertyChanged(() => HeaderMode);

                Messenger.Default.Send(HeaderMode, nameof(ScrollHeaderMode));
            }
        }

        public AppSettings()
        {
            localSettings = ApplicationData.Current.LocalSettings;

            var language = ApplicationLanguages.PrimaryLanguageOverride?.Trim();
            if (string.IsNullOrEmpty(language))
            {
                var primary = ApplicationLanguages.Languages[0];
                SaveSettings(nameof(Language), primary.StartsWith("zh") ? LANGUAGE_ZH : LANGUAGE_EN);
            }
        }

        public async Task<StorageFolder> GetSavingFolderAsync()
        {
            var folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Attention", CreationCollisionOption.OpenIfExists);
            return folder;
        }

        public async Task<StorageFolder> GetTemporaryFolderAsync()
        {
            var folder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync("AttentionTemp", CreationCollisionOption.OpenIfExists);
            return folder;
        }

        private void SaveSettings(string key, object value)
        {
            localSettings.Values[key] = value;
        }
        private T ReadSettings<T>(string key, T defaultValue)
        {
            if (localSettings.Values.ContainsKey(key))
            {
                return (T)localSettings.Values[key];
            }
            if (defaultValue != null)
            {
                return defaultValue;
            }
            return default;
        }
    }
}
