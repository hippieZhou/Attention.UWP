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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace HENG.ViewModels
{
    public class NewestViewModel : PhotoViewModel<NewestItemSource, PaperItem>
    {
    }

    public class NewestItemSource : IIncrementalSource<PaperItem>
    {
        public async Task<IEnumerable<PaperItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await Singleton<DataService>.Instance.GetNewestAsync(++pageIndex, pageSize, cancellationToken);

            if (items != null)
            {
                LiveTileService.UpdateLiveTile(items.Take(3).Select(p => p.Urls.Thumb));
            }

            return items;
        }
    }
}
