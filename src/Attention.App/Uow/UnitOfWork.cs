using Attention.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task Rollback()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
