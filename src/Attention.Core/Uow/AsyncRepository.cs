using Attention.Core.Context;
using Attention.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attention.Core.Uow
{
    public class AsyncRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : AuditableEntity
    {
        private readonly IDateTime _dateTime;
        private readonly IApplicationDbContext _dbContext;
        public IQueryable<TEntity> Table => _dbContext.Conn.Table<TEntity>().AsQueryable();

        public AsyncRepository(IDateTime dateTime, IApplicationDbContext dbContext)
        {
            _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            entity.Created = _dateTime.Now;

            using (var conn = _dbContext.Conn)
            {
                conn.Insert(entity);
                conn.Commit();
            }
            await Task.Yield();
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Created = _dateTime.Now;
            }

            using (var conn = _dbContext.Conn)
            {
                conn.InsertAll(entities);
                conn.Commit();
            }
            await Task.Yield();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            using (var conn = _dbContext.Conn)
            {
                conn.Delete(entity);
                conn.Commit();
            }
            await Task.Yield();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            entity.LastModified = _dateTime.Now;

            using (var conn = _dbContext.Conn)
            {
                conn.Update(entity);
                conn.Commit();
            }
            await Task.Yield();
        }

        public virtual async Task<TEntity> FindByIdAsync(Guid id)
        {
            using (var conn = _dbContext.Conn)
            {
                return await Task.FromResult(conn.Find<TEntity>(id));
            }
        }
    }
}
