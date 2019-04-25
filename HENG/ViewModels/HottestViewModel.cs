using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace HENG.ViewModels
{
    public class HottestViewModel : ViewModelBase
    {
        private IncrementalLoadingCollection<HottestItemSource, PaperItem> _photos;
        public IncrementalLoadingCollection<HottestItemSource, PaperItem> Photos
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
                    _loadedCommand = new RelayCommand(() =>
                    {
                        if (Photos == null)
                        {
                            Photos = new IncrementalLoadingCollection<HottestItemSource, PaperItem>(20,
                                () =>
                                {
                                    FooterVisibility = Visibility.Visible;
                                },
                                () =>
                                {
                                    FooterVisibility = Visibility.Collapsed;
                                }, ex => { });
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

    public class HottestItemSource : IIncrementalSource<PaperItem>
    {
        public async Task<IEnumerable<PaperItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await Singleton<DataService>.Instance.GetHottestAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }
    }
}
