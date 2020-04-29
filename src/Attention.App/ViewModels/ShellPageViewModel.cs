using Attention.App.Events;
using Attention.App.Views;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using muxc = Microsoft.UI.Xaml.Controls;

namespace Attention.App.ViewModels
{
    public class ShellPageViewModel : ViewModelBase
    {
        private readonly ILoggerFacade _logger;
        private readonly IResourceLoader _resourceLoader;
        private readonly IEventAggregator _eventAggregator;

        private Frame _shellFrame;
        private muxc.NavigationView _shellNav;
        private InAppNotification _inAppNotification;

        public ShellPageViewModel(
            ILoggerFacade logger,
            IResourceLoader resourceLoader,
            IEventAggregator eventAggregator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));          
            _resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

        }

        private bool _isBackEnabled;
        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { SetProperty(ref _isBackEnabled, value); }
        }

        private object _header;
        public object Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        private ObservableCollection<muxc.NavigationViewItemBase> _primaryItems;
        public ObservableCollection<muxc.NavigationViewItemBase> PrimaryItems
        {
            get { return _primaryItems ?? (_primaryItems = new ObservableCollection<muxc.NavigationViewItemBase>()); }
            set { SetProperty(ref _primaryItems, value); }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<RaisedExceptionEvent>().Subscribe(ex =>
                        {
                            _logger.Log(ex.ToString(), Category.Exception, Priority.High);
                        });
                        _eventAggregator.GetEvent<NotificationEvent>().Subscribe(text =>
                        {
                            _inAppNotification?.Show(text, 2000);
                        });

                        PrimaryItems.Clear();

                        PrimaryItems.Add(new muxc.NavigationViewItemSeparator());
                        PrimaryItems.Add(new muxc.NavigationViewItemHeader() { Content = _resourceLoader.GetString("shellNav_menu") });
                        PrimaryItems.Add(new muxc.NavigationViewItem() { Content = _resourceLoader.GetString("shellNav_home"), Icon = new SymbolIcon(Symbol.Home), Tag = typeof(HomePage) });
                        PrimaryItems.Add(new muxc.NavigationViewItem() { Content = _resourceLoader.GetString("shellNav_download"), Icon = new SymbolIcon(Symbol.Download), Tag = typeof(DownloadPage) });

                        var first = PrimaryItems.OfType<muxc.NavigationViewItem>().FirstOrDefault();
                        _shellFrame.Navigate(Type.GetType(first.Tag.ToString()));
                    });
                }
                return _loadCommand;
            }
        }

        private ICommand _itemInvokedCommand;
        public ICommand ItemInvokedCommand
        {
            get
            {
                if (_itemInvokedCommand == null)
                {
                    _itemInvokedCommand = new DelegateCommand<muxc.NavigationViewItemInvokedEventArgs>(args =>
                    {
                        var pageType = args.IsSettingsInvoked ? typeof(SettingsPage) : Type.GetType(args.InvokedItemContainer.Tag.ToString());
                        if (pageType != null && SelectedItem != args.InvokedItemContainer)
                        {
                            _shellFrame.Navigate(pageType);
                        }
                    });
                }
                return _itemInvokedCommand;
            }
        }

        private ICommand _backRequestedCommand;
        public ICommand BackRequestedCommand
        {
            get
            {
                if (_backRequestedCommand == null)
                {
                    _backRequestedCommand = new DelegateCommand(() =>
                    {
                        _shellFrame.GoBack();
                    });
                }
                return _backRequestedCommand;
            }
        }

        public void Initialize(muxc.NavigationView shellNav, Frame frame, InAppNotification inAppNotification)
        {
            _shellNav = shellNav ?? throw new ArgumentNullException(nameof(shellNav));
            _shellFrame = frame ?? throw new ArgumentNullException(nameof(frame));
            _shellFrame.Navigated += (sender, e) =>
            {
                IsBackEnabled = _shellFrame.CanGoBack;
                SelectedItem = e?.SourcePageType == typeof(SettingsPage)
                    ? _shellNav.SettingsItem
                    : PrimaryItems.OfType<muxc.NavigationViewItem>().FirstOrDefault(x => x.Tag.ToString() == e?.SourcePageType.ToString());
                Header = SelectedItem is muxc.NavigationViewItem navItem ? navItem.Content : (default);
            };
            _inAppNotification = inAppNotification;
        }
    }
}
