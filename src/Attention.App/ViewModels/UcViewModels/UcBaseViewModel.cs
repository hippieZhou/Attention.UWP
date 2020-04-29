using Prism.Windows.Mvvm;
using Windows.UI.Xaml;

namespace Attention.App.ViewModels.UcViewModels
{
    public class UcBaseViewModel : ViewModelBase
    {
        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }
    }
}
