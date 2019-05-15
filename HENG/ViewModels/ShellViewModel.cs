using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using HENG.Helpers;
using HENG.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

namespace HENG.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly NavigationService _navService;
        private muxc.NavigationView _navView;
        private Page _detailView;
        private Grid _notifGrid;

        public ShellViewModel(INavigationService navigationService)
        {
            _navService = (NavigationService)navigationService;

            Messenger.Default.Register<NotificationMessageAction<string>>(this, HandleNotificationMessage);

            Messenger.Default.Register<GenericMessage<object>>(this, item =>
            {
                Photo = item.Content;
                _detailView.Visibility = Visibility.Visible;
            });
        }

        private void HandleNotificationMessage(NotificationMessageAction<string> message)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                NotificationMessage = message.Notification;
                _notifGrid.Visibility = Visibility.Visible;
                message.Execute("操作成功");
                await Task.Delay(TimeSpan.FromSeconds(1));
                _notifGrid.Visibility = Visibility.Collapsed;
            });
        }

        public void Initialize(Frame shellFrame, muxc.NavigationView navView, DetailView detailView, Grid notifGrid)
        {
            _navService.CurrentFrame = shellFrame;
            _navService.CurrentFrame.Navigated += Frame_Navigated;

            _navView = navView;
            _navView.BackRequested += OnBackRequested;

            _detailView = detailView;
            _detailView.Visibility = Visibility.Collapsed;

            _notifGrid = notifGrid;
        }

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

        private object _header;
        public object Header
        {
            get { return _header; }
            set { Set(ref _header, value); }
        }

        private object _photo;
        public object Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }

        private string _notificationMessage;
        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set { Set(ref _notificationMessage, value); }
        }

        private bool _refreshIsEnabled;
        public bool RefreshIsEnabled
        {
            get { return _refreshIsEnabled; }
            set { Set(ref _refreshIsEnabled, value); }
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() =>
                    {
                        var first = _navView.MenuItems.OfType<muxc.NavigationViewItem>().FirstOrDefault();
                        //var first = _navView.MenuItems.OfType<muxc.NavigationViewItem>().FirstOrDefault(p=>NavHelper.GetNavigateTo(p) == typeof(LocalViewModel).FullName);
                        if (first != null)
                        {
                            var pageKey = NavHelper.GetNavigateTo(first);
                            _navService.NavigateTo(pageKey);
                        }
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _itemInvokedCommand;
        public ICommand ItemInvokedCommand
        {
            get
            {
                if (_itemInvokedCommand == null)
                {
                    _itemInvokedCommand = new RelayCommand<muxc.NavigationViewItemInvokedEventArgs>(args =>
                    {
                        if (args.IsSettingsInvoked)
                        {
                            var has = _navService.CurrentPageKey == typeof(SettingsViewModel).FullName;
                            if (!has)
                            {
                                _navService.NavigateTo(typeof(SettingsViewModel).FullName);
                            }
                            return;
                        }

                        var item = _navView.MenuItems.OfType<muxc.NavigationViewItem>()
                        .FirstOrDefault(menuItem => (string)menuItem.Content == args.InvokedItem as string);
                        if (item != null)
                        {
                            var pageKey = item.GetValue(NavHelper.NavigateToProperty) as string;
                            var has = _navService.GetKeyForPage(_navService.CurrentFrame.CurrentSourcePageType) == pageKey;
                            if (!has)
                            {
                                _navService.NavigateTo(pageKey);
                            }
                        }
                    });
                }
                return _itemInvokedCommand;
            }
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand(() => 
                    {
                        _detailView.Visibility = Visibility.Collapsed;
                    });
                }
                return _backCommand; }
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navService.CanGoBack;

            if (e.SourcePageType == typeof(SettingsView))
            {
                Header = null;
                Selected = _navView.SettingsItem;
                RefreshIsEnabled = false;
            }
            else
            {
                var item = _navView.MenuItems.OfType<muxc.NavigationViewItem>().FirstOrDefault(p => IsMenuItemForPageType(p, e.SourcePageType));
                Selected = item;
                Header = item.Content;
                RefreshIsEnabled = true;
            }

            bool IsMenuItemForPageType(muxc.NavigationViewItem menuItem, Type sourcePageType)
            {
                var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
                var navigaedPageKey = _navService.GetKeyForPage(sourcePageType);

                return pageKey == navigaedPageKey;
            }
        }

        private void OnBackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
        {
            _navService.GoBack();
        }
    }
}