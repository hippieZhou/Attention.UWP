using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
using GalaSoft.MvvmLight.Threading;

namespace HENG.ViewModels
{
    public class SkylandViewModel : PhotoViewModel<SkyItemSource, PaperItem>
    {
    }

    public class SkyItemSource : IIncrementalSource<PaperItem>
    {
        public async Task<IEnumerable<PaperItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await Singleton<DataService>.Instance.GetSkyAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }
    }
}
