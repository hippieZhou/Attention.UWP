using Attention.App.Businesss;
using Attention.App.Extensions;
using Attention.App.Models;
using Attention.App.ViewModels.UcViewModels;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Prism.Commands;
using Prism.Logging;
using Prism.Windows.Mvvm;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.App.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly ILoggerFacade _logger;
        private AdaptiveGridView _adaptiveGV;

        private IncrementalLoadingCollection<WallpaperItemSource, WallpaperDto> _wallpapers;
        public IncrementalLoadingCollection<WallpaperItemSource, WallpaperDto> Wallpapers
        {
            get { return _wallpapers; }
            set { SetProperty(ref _wallpapers, value); }
        }
        public HomePageViewModel(ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private Visibility _errorVisibility = Visibility.Collapsed;
        public Visibility ErrorVisibility
        {
            get { return _errorVisibility; }
            set { SetProperty(ref _errorVisibility, value); }
        }

        private WallpaperExploreViewModel _exploreViewModel;
        public WallpaperExploreViewModel ExploreViewModel
        {
            get { return _exploreViewModel; }
            set { SetProperty(ref _exploreViewModel, value); }
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


                        ExploreViewModel = new WallpaperExploreViewModel();
                        ExploreViewModel.Visibility = Visibility.Collapsed;


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


                        Wallpapers = new IncrementalLoadingCollection<WallpaperItemSource, WallpaperDto>(10, () =>
                        {
                            ErrorVisibility = Visibility.Collapsed;
                        }, () =>
                        {
                            ErrorVisibility = Visibility.Collapsed;
                        }, ex =>
                        {
                            _logger.Log(ex.ToString(), Category.Exception, Priority.High);
                            ErrorVisibility = Visibility.Visible;
                        });
                    });
                }
                return _loadCommand;
            }
        }

        private ICommand _exploreCommand;
        public ICommand ExploreCommand
        {
            get
            {
                if (_exploreCommand == null)
                {
                    _exploreCommand = new DelegateCommand(() =>
                    {
                        ExploreViewModel.SwitchCommand.Execute(Visibility.Visible);
                    });
                }
                return _exploreCommand;
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
                    _itemClickCommand = new DelegateCommand<WallpaperDto>(entity =>
                    {
                        if (_adaptiveGV.ContainerFromItem(entity) is GridViewItem container)
                        {
                            container.Opacity = 0.0d;
                            var animation = container.CreateForwardAnimation(_adaptiveGV, entity, () =>
                           {
                               CardViewModel.AvatarVisibility = Visibility.Visible;
                               CardViewModel.FooterVisibility = Visibility.Visible;
                           });
                            CardViewModel.TryStartForwardAnimation(entity, animation);
                        }
                    });
                }
                return _itemClickCommand;
            }
        }
    }
}
