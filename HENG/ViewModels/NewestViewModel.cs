using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace HENG.ViewModels
{
    public class NewestViewModel : ViewModelBase
    {
        private IncrementalLoadingCollection<NewestItemSource, PaperItem> _photos;
        public IncrementalLoadingCollection<NewestItemSource, PaperItem> Photos
        {
            get { return _photos; }
            set { Set(ref _photos, value); }
        }

        private Visibility _footerVisibility = Visibility.Collapsed;
        public Visibility FooterVisibility
        {
            get { return _footerVisibility; }
            set { Set(ref _footerVisibility, value); }
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(async () =>
                    {
                        if (Photos == null)
                        {
                            await DispatcherHelper.RunAsync(() =>
                            {
                                Photos = new IncrementalLoadingCollection<NewestItemSource, PaperItem>(20,
                                    () =>
                                    {
                                        FooterVisibility = Visibility.Visible;
                                    },
                                    () =>
                                    {
                                        FooterVisibility = Visibility.Collapsed;
                                    }, ex => { });
                            });
                        }
                    });
                }
                return _loadedCommand;
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
                        await Photos.RefreshAsync();
                    });
                }
                return _refreshCommand;
            }
        }
    }

    public class NewestItemSource : IIncrementalSource<PaperItem>
    {
        public async Task<IEnumerable<PaperItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await Singleton<DataService>.Instance.GetNewestAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }
    }
}
