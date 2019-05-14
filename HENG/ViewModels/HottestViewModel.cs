using HENG.Clients;
using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using Microsoft.Toolkit.Collections;
using System;
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
        private readonly PaperClient _client;
        public HottestItemSource()
        {
            _client = ViewModelLocator.Current.ServiceProvider.GetService(typeof(PaperClient)) as PaperClient;
            if (_client == null)
            {
                throw new NotSupportedException($"{typeof(BingClient).FullName} is NULL");
            }
        }
        public async Task<IEnumerable<PaperItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await _client.GetHottestAsync(++pageIndex, pageSize, cancellationToken);
            return items;
        }
    }
}
