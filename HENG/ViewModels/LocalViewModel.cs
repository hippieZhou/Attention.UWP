using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Models;
using Microsoft.Toolkit.Collections;
using PixabaySharp.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml;

namespace HENG.ViewModels
{
    public class LocalViewModel : ViewModelBase
    {
        private IncrementalLoadingCollection<DownloadSource, DownloadItem> _photos;
        public IncrementalLoadingCollection<DownloadSource, DownloadItem> Photos
        {
            get { return _photos; }
            set { Set(ref _photos, value); }
        }

        private Visibility _loadingVisibility = Visibility.Visible;
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

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() =>
                    {
                        if (Photos == null)
                        {
                            Photos = new IncrementalLoadingCollection<DownloadSource, DownloadItem>(5,
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
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                if (_shareCommand == null)
                {
                    _shareCommand = new RelayCommand<DownloadItem>(item =>
                    {

                    });
                }
                return _shareCommand;
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand<DownloadItem>(item =>
                    {

                    });
                }
                return _deleteCommand;
            }
        }
    }

    public class DownloadSource : IIncrementalSource<DownloadItem>
    {
        private readonly List<DownloadItem> _downloads;

        public DownloadSource()
        {
            IEnumerable<ImageItem> items = ViewModelLocator.Current.Db.GetAllItems<ImageItem>();
            _downloads = new List<DownloadItem>(from p in items select new DownloadItem(p));
        }

        public async Task<IEnumerable<DownloadItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = (from p in _downloads select p).Skip(pageIndex * pageSize).Take(pageSize);
            await Task.Delay(1000);
            return result;
        }
    }
}
