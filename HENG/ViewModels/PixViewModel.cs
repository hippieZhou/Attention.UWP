using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Core.Services;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using System;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Xaml.Controls;

namespace HENG.ViewModels
{
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

        public virtual void Initialize(ListViewBase listView,int itemsPerPage = 20)
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
                    _itemClickCommand = new RelayCommand<IType>(item => NavToByItem(item));
                }
                return _itemClickCommand;
            }
        }

        protected virtual void NavToByItem(IType item) => Messenger.Default.Send(item, item.GetType());

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
                return _refreshCommand; }
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand<IType>(item => 
                    {

                    });
                }
                return _downloadCommand;
            }
        }
    }
}
