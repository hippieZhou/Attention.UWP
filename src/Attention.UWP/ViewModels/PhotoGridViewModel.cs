using Attention.UWP.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI;
using PixabaySharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.ViewModels
{
    public class PhotoGridViewModel : PixViewModel<PhotoItemSource, ImageItem>
    {
        public PhotoGridViewModel()
        {
            Items = new IncrementalLoadingCollection<PhotoItemSource, ImageItem>(new PhotoItemSource(),
                20, () =>
                 {
                     LoadingVisibility = Visibility.Visible;

                     ErrorVisibility = Visibility.Collapsed;
                     RefreshVisibility = Visibility.Collapsed;
                     NotFoundVisibility = Visibility.Collapsed;
                 }, () =>
                 {
                     RefreshVisibility = Visibility.Visible;

                     LoadingVisibility = Visibility.Collapsed;
                     ErrorVisibility = Visibility.Collapsed;
                     NotFoundVisibility = Items == null || Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                 }, ex =>
                 {
                     ErrorVisibility = Visibility.Visible;
                     LoadingVisibility = Visibility.Collapsed;
                     RefreshVisibility = Visibility.Collapsed;
                     NotFoundVisibility = Visibility.Collapsed;
                 });

            Items.CollectionChanged += (sender, e) =>
            {
                NotFoundVisibility = Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            };

            Messenger.Default.Register<bool>(this, nameof(App.Settings.LiveTitle), async enabled =>
            {
                AppListEntry entry = (await Package.Current.GetAppListEntriesAsync())[0];
                bool isPinned = await StartScreenManager.GetDefault().RequestAddAppListEntryAsync(entry);
                if (isPinned && enabled)
                {
                    IEnumerable<string> images = from p in Items.Take(5) select p.PreviewURL;
                    LiveTileHelper.UpdateLiveTile(images);
                }
                else
                {
                    LiveTileHelper.CleanUpTile();
                }
            });
        }
    }

    public class PhotoItemSource : IIncrementalSource<ImageItem>
    {
        private readonly bool _loadInMemory;

        public PhotoItemSource(bool loadInMemory = true)
        {
            _loadInMemory = loadInMemory;

            ImageCache.Instance.CacheDuration = TimeSpan.FromHours(24);
            ImageCache.Instance.MaxMemoryCacheCount = loadInMemory ? 200 : 0;
        }

        public async Task<IEnumerable<ImageItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = await ViewModelLocator.Current.Pixabay.QueryImagesAsync(page: ++pageIndex, per_page: pageSize, App.Settings.Filter);
            if (result?.Images != null)
            {
                Parallel.ForEach(result.Images, async p =>
                {
                    await ImageCache.Instance.PreCacheAsync(new Uri(p.LargeImageURL), false, _loadInMemory);
                });
                return result.Images;
            }
            else
            {
                return new List<ImageItem>();
            }
        }
    }

    public class PixViewModel<TSource, IType> : ViewModelBase where TSource : IIncrementalSource<IType>
    {
        public GridView ViewContainer { get; private set; }

        private IType _selected;
        public IType Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        private IncrementalLoadingCollection<TSource, IType> _items;
        public IncrementalLoadingCollection<TSource, IType> Items
        {
            get => _items;
            protected set => Set(ref _items, value);
        }

        private Visibility _loadingVisibility = Visibility.Visible;
        public Visibility LoadingVisibility
        {
            get => _loadingVisibility;
            set => Set(ref _loadingVisibility, value);
        }

        private Visibility _errorVisibility = Visibility.Collapsed;
        public Visibility ErrorVisibility
        {
            get => _errorVisibility;
            set => Set(ref _errorVisibility, value);
        }

        private Visibility _notFoundVisibility = Visibility.Collapsed;
        public Visibility NotFoundVisibility
        {
            get => _notFoundVisibility;
            set => Set(ref _notFoundVisibility, value);
        }

        private Visibility _refreshVisibility = Visibility.Collapsed;
        public Visibility RefreshVisibility
        {
            get => _refreshVisibility;
            set => Set(ref _refreshVisibility, value);
        }

        public void Initialize(GridView view) => ViewContainer = view;

        protected ICommand _loadedCommand;
        public virtual ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() =>
                    {
                        ViewContainer.Visibility = Visibility.Visible;
                    });
                }
                return _loadedCommand;
            }
        }

        protected ICommand _itemClickCommand;
        public ICommand ItemClickCommand
        {
            get
            {
                if (_itemClickCommand == null)
                {
                    _itemClickCommand = new RelayCommand<IType>(item =>
                    {
                        if (ViewContainer.ContainerFromItem(item) is GridViewItem container)
                        {
                            Selected = item;

                            ViewModelLocator.Current.Main.Forward(container);
                        }
                    });
                }
                return _itemClickCommand;
            }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(async () =>
                    {
                        RefreshVisibility = Visibility.Collapsed;

                        await Items.RefreshAsync();
                    });
                }
                return _refreshCommand;
            }
        }
    }
}
