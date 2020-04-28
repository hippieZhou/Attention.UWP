using Attention.Core.Context;
using Attention.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attention.Core.Uow
{
    public class AsyncRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : AuditableEntity
    {
        private readonly ApplicationDbContext _dbContext;
        protected DbSet<TEntity> Entities => _dbContext.DbSet<TEntity>();
        public IQueryable<TEntity> Table => Entities;

        public AsyncRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Entities.AddRangeAsync(entities);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await Task.FromResult(Entities.Remove(entity));
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await Task.Yield();
        }

        public virtual async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await Entities.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
