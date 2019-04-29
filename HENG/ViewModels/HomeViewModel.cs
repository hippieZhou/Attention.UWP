﻿using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using System.Linq;
using Microsoft.Toolkit.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            if (items != null)
            {
                LiveTileService.UpdateLiveTile(items.Take(3).Select(p => p.Url));
            }
            return items;
        }
    }
}
