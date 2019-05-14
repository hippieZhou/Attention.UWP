using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using Microsoft.Toolkit.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HENG.ViewModels
{
    public class GirlsViewModel : PhotoViewModel<GirlItemSource, PaperItem>
    {
    }

    public class GirlItemSource : IIncrementalSource<PaperItem>
    {
        public async Task<IEnumerable<PaperItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await Singleton<DataService>.Instance.GetItemsForGirlsAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }
    }
}
