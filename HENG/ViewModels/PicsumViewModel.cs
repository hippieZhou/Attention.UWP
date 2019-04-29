using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using Microsoft.Toolkit.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HENG.ViewModels
{
    public class PicsumViewModel : PhotoViewModel<PicsumItemSource, PicsumItem>
    {
    }

    public class PicsumItemSource : IIncrementalSource<PicsumItem>
    {
        public async Task<IEnumerable<PicsumItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await Singleton<DataService>.Instance.GetPicsumAsync(++pageIndex, pageSize, cancellationToken);
            if (items != null)
            {
                LiveTileService.UpdateLiveTile(items.Take(3).Select(p => p.Thumb));
            }
            return items;
        }
    }
}
