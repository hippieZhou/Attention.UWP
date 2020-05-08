using Attention.Core.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Core.Bus
{
    public interface IMediatorHandler
    {
        Task<TRESPONSE> Send<TRESPONSE>(Command<TRESPONSE> request, CancellationToken cancellationToken = default);
    }
}
