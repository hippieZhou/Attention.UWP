using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HENG.UWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(async () =>
                    {
                        await Task.Yield();
                    });
                }
                return _loadedCommand;
            }
        }
    }
}
