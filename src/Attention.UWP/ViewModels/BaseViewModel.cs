using Attention.UWP.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.UWP.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility

        {
            get { return _visibility; }
            set { Set(ref _visibility, value); }
        }

        protected ICommand _backCommand;
        public virtual ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand(() =>
                    {
                        Visibility = Visibility.Collapsed;
                        ViewModelLocator.Current.Shell.MainElement.PlayScaleSpringAnimation(false);
                    });
                }
                return _backCommand;
            }
        }
    }
}
