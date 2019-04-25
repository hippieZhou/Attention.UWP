using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

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
                            Photos = new IncrementalLoadingCollection<NewestItemSource, PaperItem>(20);
                        }
                    });
                }
                return _loadedCommand;
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
