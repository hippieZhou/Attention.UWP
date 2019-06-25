using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using System;

namespace HENG.App.ViewModels
{
    public class MoreViewModel:ViewModelBase
    {
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
