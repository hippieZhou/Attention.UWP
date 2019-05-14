using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HENG.Clients
{
    public interface IBaseClient<TSource>
    {
        Task<IEnumerable<TSource>> GetItemsAsync(int page, int per_page = 10, CancellationToken cancellationToken = default(CancellationToken));
    }
}
