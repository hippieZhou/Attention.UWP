using Prism.Commands;
using Prism.Windows.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Logging;
using System;
using Windows.UI.Xaml.Controls;
using muxc = Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Attention.App.Views;
using System.Linq;

namespace Attention.App.ViewModels
{
    public class ShellPageViewModel: ViewModelBase
    {
        private const string PanoramicStateName = "PanoramicState";
        private const string WideStateName = "WideState";
        private const string NarrowStateName = "NarrowState";
        private const double WideStateMinWindowWidth = 640;
        private const double PanoramicStateMinWindowWidth = 1024;
        private muxc.NavigationView _shellNav;
        private Frame _shellFrame;

        private readonly ILoggerFacade _logger;
        public ShellPageViewModel(ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { SetProperty(ref _isPaneOpen, value); }
        }

        private bool _isBackEnabled;
        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { SetProperty(ref _isBackEnabled, value); }
        }

        private muxc.NavigationViewPaneDisplayMode _displayMode = muxc.NavigationViewPaneDisplayMode.LeftCompact;
        public muxc.NavigationViewPaneDisplayMode DisplayMode
        {
            get { return _displayMode; }
            set { SetProperty(ref _displayMode, value); }
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

        //private muxc.NavigationViewItem _selectedItem;
        //public muxc.NavigationViewItem SelectedItem
        //{
        //    get { return _selectedItem; }
        //    set { SetProperty(ref _selectedItem, value); }
        //}

        private ICommand _stateChangedCommand;
        public ICommand StateChangedCommand
        {
            get
            {
                if (_stateChangedCommand == null)
                {
                    _stateChangedCommand = new DelegateCommand<VisualStateChangedEventArgs>(args => 
                    {
                        GoToState(args.NewState.Name);
                    });
                }
                return _stateChangedCommand;
            }
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
                        PrimaryItems.Clear();
                        PrimaryItems.Add(new muxc.NavigationViewItemSeparator());
                        PrimaryItems.Add(new muxc.NavigationViewItemHeader() { Content = "菜单" });
                        PrimaryItems.Add(new muxc.NavigationViewItem() { Content = "首页", Icon = new SymbolIcon(Symbol.Home), Tag = typeof(HomePage) });
                        PrimaryItems.Add(new muxc.NavigationViewItem() { Content = "下载", Icon = new SymbolIcon(Symbol.Download), Tag = typeof(DownloadPage) });
                        var first = PrimaryItems.FirstOrDefault(x => x.GetType() == typeof(muxc.NavigationViewItem)) as muxc.NavigationViewItem;
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
                        if (pageType != null && _shellNav.SelectedItem != args.InvokedItemContainer)
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

        public void Initialize(muxc.NavigationView shellNav, Frame frame)
        {
            _shellNav = shellNav ?? throw new ArgumentNullException(nameof(shellNav));
            _shellFrame = frame ?? throw new ArgumentNullException(nameof(frame));
            _shellFrame.Navigated += (sender, e) => 
            {
                IsBackEnabled = _shellFrame.CanGoBack;
                var selectedItem = PrimaryItems
                .Where(x => x is muxc.NavigationViewItem navItem)
                .Cast<muxc.NavigationViewItem>()
                .FirstOrDefault(x => x.Tag.ToString() == e?.SourcePageType.ToString());
                if (selectedItem != null)
                {
                    _shellNav.SelectedItem = selectedItem;
                    Header = e?.SourcePageType == typeof(SettingsPage) ? "设置" : selectedItem?.Content;
                }
            };
            InitializeState(Window.Current.Bounds.Width);
        }

        private void InitializeState(double windowWith)
        {
            if (windowWith < WideStateMinWindowWidth)
            {
                GoToState(NarrowStateName);
            }
            else if (windowWith < PanoramicStateMinWindowWidth)
            {
                GoToState(WideStateName);
            }
            else
            {
                GoToState(PanoramicStateName);
            }
        }

        private void GoToState(string stateName)
        {
            switch (stateName)
            {
                case PanoramicStateName:
                    DisplayMode = muxc.NavigationViewPaneDisplayMode.Auto;
                    IsPaneOpen = true;
                    break;
                case WideStateName:
                    DisplayMode = muxc.NavigationViewPaneDisplayMode.LeftCompact;
                    IsPaneOpen = true;
                    break;
                case NarrowStateName:
                    DisplayMode = muxc.NavigationViewPaneDisplayMode.LeftMinimal;
                    IsPaneOpen = false;
                    break;
                default:
                    break;
            }
        }
    }
}
