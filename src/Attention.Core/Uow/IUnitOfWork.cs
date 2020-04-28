using System;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Core.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit(CancellationToken cancellationToken);
        Task Rollback();
    }
}
