using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using PixabaySharp.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace HENG.UWP.ViewModels
{
    public class PhotoGridViewModel : PixViewModel<PhotoItemSource, ImageItem>
    {
        public override ICommand ItemClickCommand
        {
            get
            {
                if (_itemClickCommand == null)
                {
                    _itemClickCommand = new RelayCommand<ImageItem>(item =>
                    {
                        if (ViewModelLocator.Current.Home.PhotoSearchViewModel.Visibility == Visibility.Collapsed)
                        {
                            Selected = item;

                            ConnectedAnimation animation = null;
                            if (GridView.ContainerFromItem(item) is GridViewItem container)
                            {
                                container.Opacity = 0.0d;
                                animation = GridView.PrepareConnectedAnimation("forwardAnimation", Selected, "connectedElement");
                            }

                            ViewModelLocator.Current.Home.PhotoItemViewModel.TryStart(Selected, animation);
                        }
                    });
                }
                return _itemClickCommand;
            }
        }

        public async Task TryStartConnectedAnimationAsync(ImageItem photo, ConnectedAnimation animation)
        {
            Selected = photo;
            GridView.ScrollIntoView(Selected, ScrollIntoViewAlignment.Default);
            GridView.UpdateLayout();
            animation.Completed += (sender, e) => 
            {
                if (GridView.ContainerFromItem(Selected) is GridViewItem container)
                {
                    container.Opacity = 1.0d;
                }
            };
            await GridView.TryStartConnectedAnimationAsync(animation, Selected, "connectedElement");
        }
    }

    public class PhotoItemSource : IIncrementalSource<ImageItem>
    {
        public async Task<IEnumerable<ImageItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = await ViewModelLocator.Current.Pix.QueryImagesAsync(page: ++pageIndex, per_page: pageSize);
            return result != null ? result.Images : new List<ImageItem>();
        }
    }

    public class PixViewModel<TSource, IType> : ViewModelBase where TSource : IIncrementalSource<IType>
    {
        public GridView GridView { get; protected set; }

        private IType _selected;
        public IType Selected
        {
            get { return _selected; }
            protected set { Set(ref _selected, value); }
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

        protected ICommand _loadedCommand;
        public virtual ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand<GridView>(gridview =>
                    {
                        GridView = gridview;

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
                        Selected = item;
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
    }
}
