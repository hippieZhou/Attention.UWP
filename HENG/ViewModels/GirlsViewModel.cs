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

namespace HENG.ViewModels
{
    public class GirlsViewModel : ViewModelBase
    {
        private IncrementalLoadingCollection<GirlItemSource, PaperItem> _photos;
        public IncrementalLoadingCollection<GirlItemSource, PaperItem> Photos
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
                            Photos = new IncrementalLoadingCollection<GirlItemSource, PaperItem>(20);
                        }
                    });
                }
                return _loadedCommand;
            }
        }
    }

    public class GirlItemSource : IIncrementalSource<PaperItem>
    {
        public async Task<IEnumerable<PaperItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await Singleton<DataService>.Instance.GetGirlsAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }
    }
}
