using HENG.Models;
using Microsoft.Toolkit.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HENG.Helpers;
using HENG.Services;

namespace HENG.ViewModels
{
    public class SkylandViewModel : PhotoViewModel<SkyItemSource, PaperItem>
    {
    }

    public class SkyItemSource : IIncrementalSource<PaperItem>
    {
        public async Task<IEnumerable<PaperItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await Singleton<DataService>.Instance.GetItemsForSkylandAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }
    }
}
