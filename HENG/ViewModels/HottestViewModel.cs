using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using Microsoft.Toolkit.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
