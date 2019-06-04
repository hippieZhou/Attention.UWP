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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

namespace HENG.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly NavigationService _navService;
        private muxc.NavigationView _navView;
        private Grid _smokeGrid;

        public ShellViewModel(NavigationService navigationService)
        {
            _navService = navigationService;

            Messenger.Default.Register<GenericMessage<ImageItem>>(this, "forwardAnimation", item =>
            {
                if (item.Target is ConnectedAnimation animation)
                {
                    try
                    {
                        StoredItem = item.Content;
                        _smokeGrid.Visibility = Visibility.Visible;
                        animation.TryStart(_smokeGrid.FindName("destinationElement") as UIElement);
                    }
                    catch (Exception ex)
                    {
                        StoredItem = null;
                        _smokeGrid.Visibility = Visibility.Collapsed;
                        animation.Cancel();
                        Trace.WriteLine(ex);
                    }
                }
            });
        }

        public void Initialize(muxc.NavigationView navView, Frame shellFrame, Grid smokeGrid)
        {
            _navView = navView;
            _navView.BackRequested += (sender, args) => { _navService.GoBack(); };

            _navService.CurrentFrame = shellFrame;
            _navService.CurrentFrame.Navigated += Frame_Navigated;

            _smokeGrid = smokeGrid;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navService.CanGoBack;

            if (e.SourcePageType == typeof(SettingsView))
            {
                Selected = _navView.SettingsItem;
            }
            else
            {
                var item = _navView.MenuItems.OfType<muxc.NavigationViewItem>().FirstOrDefault(p => IsMenuItemForPageType(p, e.SourcePageType));
                Selected = item;
            }

            bool IsMenuItemForPageType(muxc.NavigationViewItem menuItem, Type sourcePageType)
            {
                var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
                var navigaedPageKey = _navService.GetKeyForPage(sourcePageType);

                return pageKey == navigaedPageKey;
            }
        }

        private ImageItem _storedItem;
        public ImageItem StoredItem
        {
            get { return _storedItem; }
            set { Set(ref _storedItem, value); }
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

        private ICommand _filterCommand;
        public ICommand FilterCommand
        {
            get
            {
                if (_filterCommand == null)
                {
                    _filterCommand = new RelayCommand(() =>
                    {
                        ViewModelLocator.Current.Home.HeaderUpCommand.Execute(null);
                    });
                }
                return _filterCommand;
            }
        }


        private ICommand _navToLocalCommand;
        public ICommand NavToLocalCommand
        {
            get
            {
                if (_navToLocalCommand == null)
                {
                    _navToLocalCommand = new RelayCommand(() =>
                    {
                        _navService.NavigateTo(typeof(LocalViewModel).FullName);
                    });
                }
                return _navToLocalCommand;
            }
        }

        private ICommand _launcherCommand;
        public ICommand LauncherCommand
        {
            get
            {
                if (_launcherCommand == null)
                {
                    _launcherCommand = new RelayCommand<ImageItem>(async item =>
                    {
                        if (!string.IsNullOrWhiteSpace(item?.PageURL))
                        {
                            await Launcher.LaunchUriAsync(new Uri(item.PageURL));
                        }
                    }, item => item != null);
                }
                return _launcherCommand;
            }
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand<ImageItem>(item =>
                    {
                        ViewModelLocator.Current.Home.DownloadCommand.Execute(item);
                    }, item => item != null);
                }
                return _downloadCommand; }
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand<ImageItem>(item =>
                    {
                        ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", _smokeGrid.FindName("destinationElement") as UIElement);
                        animation.Completed += (sender, e) =>
                        {
                            _smokeGrid.Visibility = Visibility.Collapsed;
                        };
                        if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                        {
                            animation.Configuration = new DirectConnectedAnimationConfiguration();
                        }

                        Messenger.Default.Send(new GenericMessage<ImageItem>(this, animation, item), "backwardsAnimation");
                    }, item => item != null);
                }
                return _backCommand;
            }
        }
    }
}
