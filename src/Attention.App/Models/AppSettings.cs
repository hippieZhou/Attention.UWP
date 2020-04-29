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
        public const string DBFile = "default.db";

        public StorageFolder LocalFolder => ApplicationData.Current.LocalFolder;
        private readonly ApplicationDataContainer _localSettings;

        public string Version
        {
            get
            {
                PackageVersion packageVersion = Package.Current.Id.Version;
                return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";
            }
        }

        public ElementTheme Theme
        {
            get { return (ElementTheme)ReadSettings(nameof(Theme), (int)ElementTheme.Default); }
            set
            {
                SaveSettings(nameof(Theme), (int)value);
                RaisePropertyChanged(nameof(Theme));
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    frameworkElement.RequestedTheme = Theme;
                }
            }
        }

        public string Language
        {
            get { return ReadSettings(nameof(Language), ApplicationLanguages.Languages[0].StartsWith("zh") ? "zh-CN" : "en-US"); }
            set
            {
                SaveSettings(nameof(Language), value);
                RaisePropertyChanged(nameof(Language));
                ApplicationLanguages.PrimaryLanguageOverride = Language;
            }
        }

        public ElementSoundPlayerState SoundPlayerState
        {
            get { return (ElementSoundPlayerState)ReadSettings(nameof(SoundPlayerState), (int)ElementSoundPlayerState.Off); }
            set
            {
                SaveSettings(nameof(SoundPlayerState), (int)value);
                RaisePropertyChanged(nameof(SoundPlayerState));
                EnableSound(SoundPlayerState);
            }
        }

        public void EnableSound(ElementSoundPlayerState soundPlayerState, bool withSpatial = false)
        {
            ElementSoundPlayer.State = soundPlayerState;
            ElementSoundPlayer.SpatialAudioMode = !withSpatial ? ElementSpatialAudioMode.Off : ElementSpatialAudioMode.On;
        }

        public AppSettings() => _localSettings = ApplicationData.Current.LocalSettings;

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
