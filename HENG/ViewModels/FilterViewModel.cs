using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace HENG.ViewModels
{
    public class FilterViewModel : ViewModelBase
    {
        private ICommand _primaryCommand;
        public ICommand PrimaryCommand
        {
            get
            {
                if (_primaryCommand == null)
                {
                    _primaryCommand = new RelayCommand(() =>
                    {

                    });
                }
                return _primaryCommand;
            }
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(() =>
                    {

                    });
                }
                return _closeCommand;
            }
        }
    }
}
