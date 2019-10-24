using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.UWP.Helpers;
using HENG.UWP.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using winui = Microsoft.UI.Xaml.Controls;

namespace HENG.UWP.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private winui.NavigationView _navigationView;
        public static NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        private bool _isBackEnabled;
        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }

        private object _selected;
        public object Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private bool _isPaneOpen = false;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private Visibility _paneFooterVisibility;
        public Visibility PaneFooterVisibility
        {
            get { return _paneFooterVisibility; }
            set { Set(ref _paneFooterVisibility, value); }
        }

        public void Initialize(Frame frame, winui.NavigationView navigationView)
        {
            _navigationView = navigationView;
            NavigationService.Frame = frame;
            NavigationService.NavigationFailed += Frame_NavigationFailed;
            NavigationService.Navigated += Frame_Navigated;
            _navigationView.BackRequested += OnBackRequested;
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            Selected = _navigationView.MenuItems
                .OfType<winui.NavigationViewItem>()
                .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));

            PaneFooterVisibility = 1 == _navigationView.MenuItems.IndexOf(Selected) ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool IsMenuItemForPageType(winui.NavigationViewItem menuItem, Type sourcePageType)
        {
            var navigatedPageKey = NavigationService.GetNameOfRegisteredPage(sourcePageType);
            var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
            return pageKey == navigatedPageKey;
        }

        private void OnBackRequested(winui.NavigationView sender, winui.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(async () => 
                    {
                        var pageKey = _navigationView.MenuItems.OfType<winui.NavigationViewItem>().FirstOrDefault().GetValue(NavHelper.NavigateToProperty) as string;
                        NavigationService.Navigate(pageKey);
                        await Task.CompletedTask;
                    });
                }
                return _loadedCommand; }
        }

        private ICommand _itemInvokedCommand;
        public ICommand ItemInvokedCommand
        {
            get
            {
                if (_itemInvokedCommand == null)
                {
                    _itemInvokedCommand = new RelayCommand<winui.NavigationViewItemInvokedEventArgs>(args =>
                    {
                        var item = _navigationView.MenuItems
                          .OfType<winui.NavigationViewItem>()
                          .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
                        var pageKey = item.GetValue(NavHelper.NavigateToProperty) as string;
                        NavigationService.Navigate(pageKey);
                    });
                }
                return _itemInvokedCommand;
            }
        }

        private ICommand _openPaneCommand;
        public ICommand OpenPaneCommand
        {
            get
            {
                if (_openPaneCommand == null)
                {
                    _openPaneCommand = new RelayCommand(() =>
                    {
                        IsPaneOpen = !IsPaneOpen;
                    });
                }
                return _openPaneCommand;
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(() =>
                    {
                        ViewModelLocator.Current.Home.SearchCommand.Execute(null);
                    });
                }
                return _searchCommand;
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
                        ViewModelLocator.Current.Home.PhotoGirdViewModel.RefreshCommand.Execute(null);
                    });
                }
                return _refreshCommand;
            }
        }
    }
}
