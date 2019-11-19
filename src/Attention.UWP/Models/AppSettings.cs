using Attention.UWP.Extensions;
using Attention.UWP.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Attention.UWP.Models
{
    public partial class AppSettings : ObservableObject
    {
        public string DbFile => Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite");

        public string Name => "AppDisplayName".GetLocalized();

        public string Version
        {
            get
            {
                PackageVersion packageVersion = Package.Current.Id.Version;
                return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";
            }
        }

        #region Theme
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

                void RefreshTheme()
                {
                    if (Window.Current.Content is FrameworkElement rootElement)
                    {
                        var theme = (ElementTheme)Enum.ToObject(typeof(ElementTheme), Theme);
                        rootElement.RequestedTheme = theme;
                        Messenger.Default.Send(theme, nameof(ElementTheme));
                    }
                }
                RefreshTheme();
            }
        }
        #endregion

        #region Language
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

                void RefreshLanguage()
                {
                    ApplicationLanguages.PrimaryLanguageOverride = Language == LANGUAGE_ZH ? "zh-CN" : "en-US";
                    Messenger.Default.Send("more_Personalized_Language_restart".GetLocalized(), nameof(AppNotification));
                }
                RefreshLanguage();
            }
        }
        #endregion

        #region HeaderMode
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

                void RefreshHeaderMode()
                {
                    ViewModelLocator.Current.Main.PhotoGridHeaderViewModel.HeaderModel =
                        (ScrollHeaderMode)Enum.ToObject(typeof(ScrollHeaderMode), HeaderMode);
                }
                RefreshHeaderMode();
            }
        }
        #endregion

        #region LiveTitle
        private const bool LIVETITLE_DEFAULT = false;
        public bool LiveTitle
        {
            get { return ReadSettings(nameof(LiveTitle), LIVETITLE_DEFAULT); }
            set
            {
                SaveSettings(nameof(LiveTitle), value);
                RaisePropertyChanged(() => LiveTitle);

                Messenger.Default.Send(LiveTitle, nameof(LiveTitle));
            }
        }
        #endregion

        #region Filter
        private readonly Filter defaultFilter = new Filter()
        {
            Query = string.Empty,
            Order = PixabaySharp.Enums.Order.Latest,
            ImageType = PixabaySharp.Enums.ImageType.All,
            Orientation = PixabaySharp.Enums.Orientation.All,
            Category = PixabaySharp.Enums.Category.Backgrounds
        };
        public Filter Filter
        {
            get
            {
                return JsonConvert.DeserializeObject<Filter>(
                    ReadSettings(nameof(Filter), JsonConvert.SerializeObject(defaultFilter)));
            }
            set
            {
                SaveSettings(nameof(Filter), JsonConvert.SerializeObject(value));
                RaisePropertyChanged(() => Filter);
            }
        }
        #endregion

        public async Task InitializeAsync()
        {
            var language = ApplicationLanguages.PrimaryLanguageOverride?.Trim();
            if (string.IsNullOrEmpty(language))
            {
                var primary = ApplicationLanguages.Languages[0];
                SaveSettings(nameof(Language), primary.StartsWith("zh") ? LANGUAGE_ZH : LANGUAGE_EN);
            }

            await Task.CompletedTask;
        }

        public async Task<StorageFolder> GetSavingFolderAsync()
        {
            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Attention", CreationCollisionOption.OpenIfExists);
            return folder;
        }

        public async Task<StorageFolder> GetTemporaryFolderAsync()
        {
            StorageFolder folder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync("ImageCache", CreationCollisionOption.OpenIfExists);
            return folder;
        }

        public async Task<StorageFolder> GetLogFolderAsync()
        {
            StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("MetroLogs", CreationCollisionOption.OpenIfExists);
            return folder;
        }
    }

    public partial class AppSettings : ObservableObject
    {
        public AppSettings() => localSettings = ApplicationData.Current.LocalSettings;

        private readonly ApplicationDataContainer localSettings;

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
