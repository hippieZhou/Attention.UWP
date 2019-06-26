using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using PixabaySharp.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Windows.UI.Xaml.Media.Animation;
using Windows.System;
using HENG.App.Models;

namespace HENG.App.ViewModels
{
    public class PhotoGridViewModel : PixViewModel<PhotoItemSource, ImageItem>
    {
        private readonly DbContext _dbContext;

        public PhotoGridViewModel(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override ICommand ItemClickCommand
        {
            get
            {
                if (_itemClickCommand == null)
                {
                    _itemClickCommand = new RelayCommand<ImageItem>(item =>
                    {
                        StoredItem = item;

                        if (StoredItem != null)
                        {
                            ConnectedAnimation animation = _masterView.PrepareConnectedAnimation("forwardAnimation", StoredItem, "connectedElement");
                            animation.Completed += (sender, e) =>
                            {
                                var element = _masterView.ContainerFromItem(StoredItem) as GridViewItem;
                                element.Opacity = 0d;
                            };

                            _detailView.Visibility = Visibility.Visible;
                            animation.TryStart(_detailView.FindName("destinationElement") as UIElement);
                        }
                    });
                }
                return _itemClickCommand;
            }
        }

        public override ICommand BackToMasterCommand
        {
            get
            {
                if (_backToMasterCommand == null)
                {
                    _backToMasterCommand = new RelayCommand(async () =>
                    {
                        ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", _detailView.FindName("destinationElement") as UIElement);
                        animation.Completed += (sender, e) =>
                        {
                            _detailView.Visibility = Visibility.Collapsed;
                        };
                        if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                        {
                            animation.Configuration = new DirectConnectedAnimationConfiguration();
                        }

                        GridViewItem element = _masterView.ContainerFromItem(StoredItem) as GridViewItem;
                        element.Opacity = 1.0d;

                        _masterView.ScrollIntoView(StoredItem, ScrollIntoViewAlignment.Default);
                        _masterView.UpdateLayout();
                        await _masterView.TryStartConnectedAnimationAsync(animation, StoredItem, "connectedElement");
                    });
                }
                return _backToMasterCommand;
            }
        }

        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private ICommand _showTipsCommand;
        public ICommand ShowTipsCommand
        {
            get
            {
                if (_showTipsCommand == null)
                {
                    _showTipsCommand = new RelayCommand(() =>
                    {
                        IsPaneOpen = !IsPaneOpen;
                    }, () => StoredItem != null);
                }
                return _showTipsCommand;
            }
        }

        private ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                {
                    _browseCommand = new RelayCommand(async () =>
                    {
                        if (!string.IsNullOrWhiteSpace(StoredItem?.PageURL))
                        {
                            await Launcher.LaunchUriAsync(new Uri(StoredItem.PageURL));
                        }
                    }, () => StoredItem != null);
                }
                return _browseCommand;
            }
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand(() =>
                    {
                        //var count = ViewModelLocator.Current.Db.InsertItem(StoredItem);
                        //if (count > 0)
                        //{
                        //    var download = new DownloadItem(StoredItem);
                        //    await DownloadService.DownloadAsync(download);
                        //}
                    }, () => StoredItem != null);
                }
                return _downloadCommand;
            }
        }
    }

    public class PhotoItemSource : IIncrementalSource<ImageItem>
    {
        public async Task<IEnumerable<ImageItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = await ViewModelLocator.Current.Pix.QueryImagesAsync(page: ++pageIndex, per_page: pageSize);
            return result != null ? result.Images : new List<ImageItem>();
            //await Task.Yield();
            //return new List<ImageItem>();
        }
    }

    public class PixViewModel<TSource, IType> : ViewModelBase where TSource : IIncrementalSource<IType>
    {
        protected GridView _masterView;
        protected Grid _detailView;

        private IType _storedItem;
        public IType StoredItem
        {
            get { return _storedItem; }
            protected set { Set(ref _storedItem, value); }
        }

        private IncrementalLoadingCollection<TSource, IType> _items;
        public IncrementalLoadingCollection<TSource, IType> Items
        {
            get { return _items; }
            set { Set(ref _items, value); }
        }

        private Visibility _headerVisibility = Visibility.Visible;
        public Visibility HeaderVisibility
        {
            get { return _headerVisibility; }
            set { Set(ref _headerVisibility, value); }
        }

        private Visibility _loadingVisibility = Visibility.Collapsed;
        public Visibility LoadingVisibility
        {
            get { return _loadingVisibility; }
            set { Set(ref _loadingVisibility, value); }
        }

        private Visibility _errorVisibility = Visibility.Collapsed;
        public Visibility ErrorVisibility
        {
            get { return _errorVisibility; }
            set { Set(ref _errorVisibility, value); }
        }

        public virtual void Initialize(GridView masterView, Grid detailView)
        {
            _masterView = masterView ?? throw new Exception("Master View Not Find");
            _detailView = detailView ?? throw new Exception("Detail View Not Find");
        }

        protected ICommand _loadedCommand;
        public virtual ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() =>
                    {
                        if (Items == null)
                        {
                            Items = new IncrementalLoadingCollection<TSource, IType>(itemsPerPage: 20,
                                async () =>
                                {
                                    await Task.Delay(1000);

                                    LoadingVisibility = Visibility.Visible;

                                    HeaderVisibility = Visibility.Collapsed;
                                    ErrorVisibility = Visibility.Collapsed;
                                },
                                () =>
                                {
                                    LoadingVisibility = Visibility.Collapsed;
                                    HeaderVisibility = Visibility.Collapsed;
                                    ErrorVisibility = Visibility.Collapsed;
                                },
                                ex =>
                                {
                                    ErrorVisibility = Visibility.Visible;

                                    HeaderVisibility = Visibility.Collapsed;
                                    ErrorVisibility = Visibility.Collapsed;
                                });
                        };
                    });
                }
                return _loadedCommand;
            }
        }

        protected ICommand _itemClickCommand;
        public virtual ICommand ItemClickCommand
        {
            get
            {
                if (_itemClickCommand == null)
                {
                    _itemClickCommand = new RelayCommand<IType>(item =>
                    {
                        StoredItem = item;
                    });
                }
                return _itemClickCommand;
            }
        }

        private ICommand _refreshCommand;
        public virtual ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(async () =>
                    {
                        await Items.RefreshAsync();
                    });
                }
                return _refreshCommand;
            }
        }

        private ICommand _backToTopCommand;
        public virtual ICommand BackToTopCommand
        {
            get
            {
                if (_backToTopCommand == null)
                {
                    _backToTopCommand = new RelayCommand(() =>
                    {
                        _masterView.FindDescendant<ScrollViewer>()?.ChangeView(0.0f, 0.0f, 1.0f, false);
                    });
                }
                return _backToTopCommand;
            }
        }

        protected ICommand _backToMasterCommand;
        public virtual ICommand BackToMasterCommand => _backToMasterCommand;
    }
}
