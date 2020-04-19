using Prism.Commands;
using Prism.Windows.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using Prism.Logging;
using System;
using Windows.UI.Xaml.Controls;
using muxc = Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Attention.App.Views;
using System.Diagnostics;

namespace Attention.App.ViewModels
{
    public class CategoryBase { }

    public class Category : CategoryBase
    {
        public string Label { get; set; }
        public Symbol Glyph { get; set; }
        public Type PageType { get; set; }

        public Category(string label, Symbol symbol, Type type)
        {
            Label = label;
            Glyph = symbol;
            PageType = type;
        }

        public override string ToString() => Label;

        public static Category FromType<T>(string label, Symbol symbol) where T : Page => new Category(label, symbol, typeof(T));
    }

    public class Separator : CategoryBase { }

    public class Header : CategoryBase
    {
        public string Label { get; set; }
    }


    public class ShellPageViewModel: ViewModelBase
    {
        private const string PanoramicStateName = "PanoramicState";
        private const string WideStateName = "WideState";
        private const string NarrowStateName = "NarrowState";
        private const double WideStateMinWindowWidth = 640;
        private const double PanoramicStateMinWindowWidth = 1024;

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

        private string _header;
        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        private ObservableCollection<CategoryBase> _primaryItems;
        public ObservableCollection<CategoryBase> PrimaryItems
        {
            get { return _primaryItems ?? (_primaryItems = new ObservableCollection<CategoryBase>()); }
            set { SetProperty(ref _primaryItems, value); }
        }

        private CategoryBase _selectedItem;
        public CategoryBase SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

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
                        PrimaryItems.Add(new Separator());
                        PrimaryItems.Add(new Header() { Label = "菜单" });
                        PrimaryItems.Add(Category.FromType<HomePage>("首页", Symbol.Home));
                        PrimaryItems.Add(Category.FromType<DownloadPage>("下载", Symbol.Download));

                        _shellFrame.Navigate(typeof(HomePage));
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
                        Trace.WriteLine(args.InvokedItem.ToString());
                        if (args.IsSettingsInvoked)
                        {
                            _shellFrame.Navigate(typeof(SettingsPage));
                        }

                        if (PrimaryItems.FirstOrDefault(x => x is Category category && category.Label == args.InvokedItem.ToString()) is Category navItem)
                        {
                            if (SelectedItem is Category currentNavItem && currentNavItem != navItem)
                            {
                                _shellFrame.Navigate(navItem.PageType);
                            }
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

        public void Initialize(Frame frame)
        {
            _shellFrame = frame;
            _shellFrame.Navigated += (sender, e) => 
            {
                IsBackEnabled = _shellFrame.CanGoBack;
                SelectedItem = PrimaryItems.FirstOrDefault(x => x is Category category && category.PageType == e?.SourcePageType);
                if (e?.SourcePageType == typeof(SettingsPage))
                {
                    Header = "设置";
                    return;
                }
                if (SelectedItem != null)
                {
                    Header = ((Category)SelectedItem).Label;
                    return;
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
