using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using System;
using HENG.App.Services;

namespace HENG.App.ViewModels
{
    public class MoreViewModel:ViewModelBase
    {
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
                    _openFolerCommand = new RelayCommand<string>(async path =>
                    {
                        var sf = await StorageFolder.GetFolderFromPathAsync(path);
                        await Launcher.LaunchFolderAsync(sf);
                    });
                }
                return _openFolerCommand;
            }
        }
    }
}
