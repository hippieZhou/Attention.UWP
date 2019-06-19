using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace HENG.ViewModels
{
    public partial class ShellViewModel : ViewModelBase
    {
        private readonly NavigationService _navService;

        public ShellViewModel(NavigationService navigationService)
        {
            _navService = navigationService;
        }

        private ICommand _itemInvokedCommand;
        public ICommand ItemInvokedCommand
        {
            get
            {
                if (_itemInvokedCommand == null)
                {
                    _itemInvokedCommand = new RelayCommand<string>(pageKey =>
                    {
                        _navService.NavigateTo(pageKey);
                    });
                }
                return _itemInvokedCommand;
            }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(() =>
                    {
                        ViewModelLocator.Current.Photo.RefreshCommand.Execute(null);
                    });
                }
                return _refreshCommand;
            }
        }

        public void Initialize()
        {
            _navService.CurrentFrame.Navigated -= OnNavigated;
            _navService.CurrentFrame.Navigated += OnNavigated;
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = _navService.CanGoBack ?
          AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (_navService.CanGoBack)
            {
                e.Handled = true;
                _navService.GoBack();
            }
        }
    }
}
