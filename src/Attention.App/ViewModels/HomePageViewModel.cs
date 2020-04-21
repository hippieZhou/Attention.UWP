using Attention.App.Businesss;
using Attention.App.Models;
using Microsoft.Toolkit.Uwp;
using Prism.Commands;
using Prism.Logging;
using Prism.Windows.Mvvm;
using System.Windows.Input;
using Windows.UI.Xaml;
using System;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Attention.App.Extensions;

namespace Attention.App.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly ILoggerFacade _logger;
        private AdaptiveGridView _adaptiveGV;

        private IncrementalLoadingCollection<WallpaperItemSource, WallpaperEntity> _wallpapers;
        public IncrementalLoadingCollection<WallpaperItemSource, WallpaperEntity> Wallpapers
        {
            get { return _wallpapers; }
            set { SetProperty(ref _wallpapers, value); }
        }

        public HomePageViewModel(ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private Visibility _loadingVisibility = Visibility.Visible;
        public Visibility LoadingVisibility
        {
            get { return _loadingVisibility; }
            set { SetProperty(ref _loadingVisibility, value); }
        }

        private Visibility _errorVisibility = Visibility.Collapsed;
        public Visibility ErrorVisibility
        {
            get { return _errorVisibility; }
            set { SetProperty(ref _errorVisibility, value); }
        }

        private WallpaperCardViewModel _cardViewModel;
        public WallpaperCardViewModel CardViewModel
        {
            get { return _cardViewModel; }
            set { SetProperty(ref _cardViewModel, value); }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand<AdaptiveGridView>(adaptiveGV =>
                    {
                        _adaptiveGV = adaptiveGV ?? throw new ArgumentNullException(nameof(adaptiveGV));
                        _adaptiveGV.SizeChanged += (sender, args) =>
                        {
                            if (args.NewSize != args.PreviousSize && _adaptiveGV.SelectedItem != null)
                            {
                                _adaptiveGV.ScrollIntoView(_adaptiveGV.SelectedItem, ScrollIntoViewAlignment.Default);
                            }
                        };

                        CardViewModel = new WallpaperCardViewModel();
                        CardViewModel.TryStartBackwardsAnimation += async (sender, args) =>
                        {
                            _adaptiveGV.ScrollIntoView(args.Item1, ScrollIntoViewAlignment.Default);
                            _adaptiveGV.UpdateLayout();
                            if (_adaptiveGV.ContainerFromItem(args.Item1) is GridViewItem container)
                            {
                                container.Opacity = 1.0d;
                            }
                            await _adaptiveGV.TryStartConnectedAnimationAsync(args.Item2, args.Item1, "connectedElement");
                        };

                        Wallpapers = new IncrementalLoadingCollection<WallpaperItemSource, WallpaperEntity>(10, () =>
                        {
                            LoadingVisibility = Visibility.Visible;
                            ErrorVisibility = Visibility.Collapsed;
                        }, () =>
                        {
                            LoadingVisibility = Visibility.Collapsed;
                            ErrorVisibility = Visibility.Collapsed;
                        }, ex =>
                        {
                            _logger.Log(ex.ToString(), Category.Exception, Priority.High);
                            LoadingVisibility = Visibility.Collapsed;
                            ErrorVisibility = Visibility.Visible;
                        });
                    });
                }
                return _loadCommand;
            }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new DelegateCommand(async () =>
                    {
                        await Wallpapers.RefreshAsync();
                    });
                }
                return _refreshCommand;
            }
        }

        protected ICommand _itemClickCommand;
        public ICommand ItemClickCommand
        {
            get
            {
                if (_itemClickCommand == null)
                {
                    _itemClickCommand = new DelegateCommand<WallpaperEntity>(entity =>
                    {
                        if (_adaptiveGV.ContainerFromItem(entity) is GridViewItem container)
                        {
                            var animation = container.CreateForwardAnimation(_adaptiveGV, entity, () => container.Opacity = 0.0d);
                            CardViewModel.TryStartForwardAnimation(entity, animation);
                        }
                    });
                }
                return _itemClickCommand; 
            }
        }
    }

    public class WallpaperCardViewModel : ViewModelBase
    {
        public event EventHandler<(WallpaperEntity, ConnectedAnimation)> TryStartBackwardsAnimation;
        private UIElement _destinationElement;

        private WallpaperEntity _entity;
        public WallpaperEntity Entity
        {
            get { return _entity; }
            set { SetProperty(ref _entity, value); }
        }

        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand<UIElement>(destinationElement =>
                    {
                        _destinationElement = destinationElement ?? throw new ArgumentNullException(nameof(destinationElement));
                        Visibility = Visibility.Collapsed;
                    });
                }
                return _loadCommand;
            }
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new DelegateCommand(() =>
                    {

                    });
                }
                return _downloadCommand;
            }
            
        }

        private ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                {
                    _browseCommand = new DelegateCommand(() =>
                    {

                    });
                }
                return _browseCommand;
            }
        }

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                if (_shareCommand == null)
                {
                    _shareCommand = new DelegateCommand(() =>
                    {

                    });
                }
                return _shareCommand;
            }
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new DelegateCommand(() =>
                    {
                        var animation = _destinationElement.CreateBackwardsAnimation(() => Visibility = Visibility.Collapsed);
                        TryStartBackwardsAnimation?.Invoke(this, (Entity, animation));
                    });
                }
                return _backCommand;
            }
        }

        public void TryStartForwardAnimation(WallpaperEntity entity, ConnectedAnimation animation)
        {
            Entity = entity;
            Visibility = Visibility.Visible;
            animation.TryStart(_destinationElement);
        }
    }
}
