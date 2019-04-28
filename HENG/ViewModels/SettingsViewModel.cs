using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Helpers;
using HENG.Services;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml;
using System;
using Windows.System;

namespace HENG.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;
        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        private string _downloadPath;
        public string DownloadPath
        {
            get { return _downloadPath; }
            set { Set(ref _downloadPath, value); }
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(async () =>
                    {
                        VersionDescription = GetVersionDescription();
                        DownloadPath = (await AppSettingService.GetDeaultDownloadPathAsync()).Path;
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _switchThemeCommand;
        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }

        private ICommand _openFolerCommand;
        public ICommand OpenFolderCommand
        {
            get
            {
                if (_openFolerCommand == null)
                {
                    _openFolerCommand = new RelayCommand(async () =>
                    {
                        var sf = await AppSettingService.GetDeaultDownloadPathAsync();
                        await Launcher.LaunchFolderAsync(sf);
                    });
                }
                return _openFolerCommand;
            }
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
