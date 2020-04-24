using Prism.Mvvm;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Attention.App.Models
{
    public class AppSettings : BindableBase
    {
        private readonly ApplicationDataContainer localSettings;

        public string Version
        {
            get
            {
                PackageVersion packageVersion = Package.Current.Id.Version;
                return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";
            }
        }

        private bool _enableHighLevel;
        public bool EnableHighLevel
        {
            get { return _enableHighLevel; }
            set
            {
                if (_enableHighLevel != value)
                {
                    SetProperty(ref _enableHighLevel, value);
                }
            }
        }

        private string _theme;
        public string Theme
        {
            get { return _theme; }
            set 
            {
                if (_theme != value)
                {
                    SetProperty(ref _theme, value);
                    if (Window.Current.Content is FrameworkElement frameworkElement 
                        && Enum.TryParse<ElementTheme>(Theme, out var theme))
                    {
                        frameworkElement.RequestedTheme = theme;
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
                    ApplicationLanguages.PrimaryLanguageOverride = Language;
                }
            }
        }

        private bool _enableLiveTitle;
        public bool EnableLiveTitle
        {
            get { return _enableLiveTitle; }
            set
            {
                if (_enableLiveTitle != value)
                {
                    SetProperty(ref _enableLiveTitle, value);
                }
            }
        }

        public async Task<StorageFolder> GetSavedFolderAsync() => await KnownFolders.PicturesLibrary.CreateFolderAsync("Attention", CreationCollisionOption.OpenIfExists);

        public async Task<StorageFolder> GetTemporaryFolderAsync() => await ApplicationData.Current.TemporaryFolder.CreateFolderAsync("ImageCache", CreationCollisionOption.OpenIfExists);
    }
}
