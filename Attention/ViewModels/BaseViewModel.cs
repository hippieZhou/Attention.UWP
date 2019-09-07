using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        private string _header;
        public string Header
        {
            get { return _header; }
            private set { Set(ref _header, value); }
        }
 
        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { Set(ref _visibility, value); }
        }

        public BaseViewModel(string header)
        {
            Header = header;
        }

        private ICommand _navBackCommand;
        public ICommand NavBackCommand
        {
            get
            {
                if (_navBackCommand == null)
                {
                    _navBackCommand = new RelayCommand(() =>
                    {
                        Visibility = Visibility.Collapsed;
                    });
                }
                return _navBackCommand;
            }
        }
    }
}
