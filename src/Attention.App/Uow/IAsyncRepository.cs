using Attention.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attention.Uow
{
    public interface IAsyncRepository<TEntity> where TEntity : AuditableEntity
    {
        IQueryable<TEntity> Table { get; }
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<TEntity> FindByIdAsync(Guid id);
    }
}
