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
    public class HottestViewModel : PhotoViewModel<HottestItemSource, PaperItem>
    {
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
