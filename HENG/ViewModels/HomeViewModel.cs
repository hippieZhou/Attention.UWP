using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HENG.Models;
using HENG.Services;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using PixabaySharp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace HENG.ViewModels
{
    public class HomeViewModel : PixViewModel<PhotoItemSource, ImageItem>
    {
        public HomeViewModel()
        {
            Messenger.Default.Register<GenericMessage<ImageItem>>(this, "backwardsAnimation", async item =>
            {
                try
                {
                    _listView.ScrollIntoView(item.Content, ScrollIntoViewAlignment.Default);
                    _listView.UpdateLayout();
                    if (item.Target is ConnectedAnimation animation)
                    {
                        await _listView.TryStartConnectedAnimationAsync(animation, item.Content, "connectedElement");
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                }
            });
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
                    });
                }
                return _loadedCommand;
            }
        }

        protected override void NavToHomeByItem(ImageItem item)
        {
            ConnectedAnimation animation = null;
            if (item != null)
            {
                animation = this._listView.PrepareConnectedAnimation("forwardAnimation", item, "connectedElement");
            }
            Messenger.Default.Send(new GenericMessage<ImageItem>(this, animation, item), "forwardAnimation");
        }
    }

    public class PhotoItemSource : IIncrementalSource<ImageItem>
    {
        public async Task<IEnumerable<ImageItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = await ViewModelLocator.Current.PxService.QueryImagesAsync(page: ++pageIndex, per_page: pageSize);
            if (result?.Images != null)
            {
                IEnumerable<string> urls = from p in result.Images.Take(5) select p.LargeImageURL;
                DataService.UpdateLiveTile(urls);
            }
            return result?.Images;
        }
    }

    public class PixViewModel<TSource, IType> : ViewModelBase where TSource : IIncrementalSource<IType>
    {
        protected ListViewBase _listView;

        private IncrementalLoadingCollection<TSource, IType> _items;
        public IncrementalLoadingCollection<TSource, IType> Items
        {
            get { return _items; }
            set { Set(ref _items, value); }
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

        public virtual void Initialize(ListViewBase listView, int itemsPerPage = 20)
        {
            _listView = listView;

            if (Items == null)
            {
                Items = new IncrementalLoadingCollection<TSource, IType>(itemsPerPage,
                    () =>
                    {
                        LoadingVisibility = Visibility.Visible;
                        ErrorVisibility = Visibility.Collapsed;
                    },
                    () =>
                    {
                        LoadingVisibility = Visibility.Collapsed;
                    },
                    ex =>
                    {
                        ErrorVisibility = Visibility.Visible;
                    });
            };
        }

        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand
        {
            get
            {
                if (_itemClickCommand == null)
                {
                    _itemClickCommand = new RelayCommand<IType>(item => NavToHomeByItem(item));
                }
                return _itemClickCommand;
            }
        }

        protected virtual void NavToHomeByItem(IType item) => throw new NotImplementedException();

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
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

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand<IType>(async item =>
                    {
                        if (item is ImageItem val)
                        {
                            var download = new DownloadItem(val);
                            await DownloadService.DownloadAsync(download);
                        }
                    });
                }
                return _downloadCommand;
            }
        }
    }
}
