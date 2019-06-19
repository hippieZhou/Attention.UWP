using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using HENG.Helpers;
using HENG.Views;
using PixabaySharp.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

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
            _navService.CurrentFrame.Navigated += (sender, e) =>
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = _navService.CanGoBack ?
                AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += (sender, e) =>
            {
                if (_navService.CanGoBack)
                {
                    _navService.GoBack();
                    e.Handled = true;
                }
            };
        }
    }
}
