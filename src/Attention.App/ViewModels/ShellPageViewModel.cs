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

namespace Attention.App.ViewModels
{
    public class CategoryBase { }

    public class Category : CategoryBase
    {
        public string Name { get; set; }
        public string Tooltip { get; set; }
        public Symbol Glyph { get; set; }
    }

    public class Separator : CategoryBase { }

    public class Header : CategoryBase
    {
        public string Name { get; set; }
    }


    public class ShellPageViewModel: ViewModelBase
    {
        private const string PanoramicStateName = "PanoramicState";
        private const string WideStateName = "WideState";
        private const string NarrowStateName = "NarrowState";
        private const double WideStateMinWindowWidth = 640;
        private const double PanoramicStateMinWindowWidth = 1024;

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

        private ObservableCollection<CategoryBase> _categories;
        public ObservableCollection<CategoryBase> Categories
        {
            get { return _categories ?? (_categories = new ObservableCollection<CategoryBase>()); }
            set { SetProperty(ref _categories, value); }
        }

        private CategoryBase _selectedItem;
        public CategoryBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value != _selectedItem)
                {
                    SetProperty(ref _selectedItem, value);
                    Header = SelectedItem is Category category ? category.Name : (default);
                }
            }
        }

        private ViewModelBase _selectedContent;
        public ViewModelBase SelectedContent
        {
            get { return _selectedContent; }
            set { SetProperty(ref _selectedContent, value); }
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
                        Categories.Clear();
                        Categories.Add(new Separator());
                        Categories.Add(new Header() { Name = "菜单" });
                        Categories.Add(new Category { Name = "首页", Glyph = Symbol.Home, Tooltip = "首页" });
                        Categories.Add(new Category { Name = "下载", Glyph = Symbol.Download, Tooltip = "下载" });
                        SelectedItem = Categories.FirstOrDefault(x => x.GetType() == typeof(Category));
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
                        if (args.IsSettingsInvoked)
                        {
                            //todo
                        }
                        var current = Categories.FirstOrDefault(x => x.GetType() == typeof(Category) && ((Category)x).Name == args.InvokedItem.ToString());
                        if (current == SelectedItem)
                        {
                            return;
                        }

                        //.FirstOrDefault(x => x.GetType() == typeof(Category) && x.);
                        //args.InvokedItem;
                    });
                }
                return _itemInvokedCommand;
            }
        }

        public void Initialize(Frame frame)
        {
            var Frame = frame;
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
                    break;
                case WideStateName:
                    DisplayMode = muxc.NavigationViewPaneDisplayMode.LeftCompact;
                    IsPaneOpen = false;
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
