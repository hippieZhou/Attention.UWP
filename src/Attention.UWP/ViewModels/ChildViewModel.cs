using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MetroLog;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.UWP.ViewModels
{
    public class ChildViewModel : ViewModelBase
    {
        protected readonly ILogger _logger;
        public ChildViewModel(ILogManager logManager) => _logger = logManager.GetLogger<ChildViewModel>();

        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                Set(ref _visibility, value);
                ViewModelLocator.Current.Shell.SpringVector3AnimationEnabled = Visibility == Visibility.Visible;
            }
        }

        protected ICommand _hideCommand;
        public virtual ICommand HideCommand
        {
            get
            {
                if (_hideCommand == null)
                {
                    _hideCommand = new RelayCommand(() =>
                    {
                        Visibility = Visibility.Collapsed;
                    });
                }
                return _hideCommand;
            }
        }
    }
}
