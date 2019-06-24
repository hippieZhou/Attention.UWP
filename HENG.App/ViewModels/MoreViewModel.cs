using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using System;
using Microsoft.Toolkit.Uwp.Extensions;
using Windows.ApplicationModel;
using HENG.App.Services;

namespace HENG.App.ViewModels
{
    public class MoreViewModel:ViewModelBase
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
