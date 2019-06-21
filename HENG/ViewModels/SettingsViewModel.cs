using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Helpers;
using HENG.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;

namespace HENG.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private int _elementTheme = (int)ThemeSelectorService.Theme;
        public int ElementTheme
        {
            get { return _elementTheme; }
            set
            {
                Set(ref _elementTheme, value);
                SwitchThemeCommand.Execute((ElementTheme)ElementTheme);
            }
        }

        private int _headerMode = (int)ScrollHeaderMode.Sticky;
        public int HeaderMode
        {
            get { return _headerMode; }
            set
            {
                Set(ref _headerMode, value);
                SwitchHeaderModeCommand.Execute((ElementTheme)HeaderMode);
            }
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
                        StorageFolder folder = await KnownFolders.PicturesLibrary.GetFolderAsync("HENG");
                        DownloadPath = folder.Path;
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
                    _switchThemeCommand = new RelayCommand<ElementTheme>(async (param) =>
                        {
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }
                return _switchThemeCommand;
            }
        }

        private ICommand _switchHeaderModeCommand;
        public ICommand SwitchHeaderModeCommand
        {
            get
            {
                if (_switchHeaderModeCommand == null)
                {
                    _switchHeaderModeCommand = new RelayCommand<ScrollHeaderMode>(mode =>
                    {
                    });
                }
                return _switchHeaderModeCommand;
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
                        var sf = await StorageFolder.GetFolderFromPathAsync(DownloadPath);
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
