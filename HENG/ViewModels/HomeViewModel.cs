using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using System;

namespace HENG.ViewModels
{
    public class HomeViewModel : PhotoViewModel<BingItemSource, BingItem>
    {

    }

    public class BingItemSource : IIncrementalSource<BingItem>
    {
        public async Task<IEnumerable<BingItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await Singleton<DataService>.Instance.GetBingsAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }
    }
}
