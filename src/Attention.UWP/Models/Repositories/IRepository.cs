using Attention.UWP.Models.Core;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Attention.UWP.Models.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity, new()
    {
        Task<int> InsertAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(object id);
        Task<IList<TEntity>> GetAllAsync();
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected string DbPath { get; private set; }

        protected SQLiteAsyncConnection AsyncDbConnection
        {
            get
            {
                var db = new SQLiteAsyncConnection(DbPath)
                {
                    Trace = true,
                    Tracer = str => Debug.WriteLine(str)
                };
                return db;
            }
        }

        public Repository(string dbPath)
        {
            DbPath = dbPath;
            Initialize();
        }

        private void Initialize()
        {
            //https://github.com/XamlBrewer/UWP-SQLite-Sample
            //https://github.com/praeclarum/sqlite-net
            using (var db = new SQLiteConnection(DbPath))
            {
                db.Tracer = str => Debug.WriteLine(str);

                // Create the table if it does not exist
                var c = db.CreateTable<TEntity>();
                var info = db.GetMapping(typeof(TEntity));
            }
        }

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            return await AsyncDbConnection.InsertAsync(entity, typeof(TEntity));
        }
        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            return await AsyncDbConnection.ExecuteAsync($"DELETE FROM {typeof(TEntity).Name} WHERE Id = ?", entity.Id); ;
        }
        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            return await AsyncDbConnection.UpdateAsync(entity, typeof(TEntity));
        }
        public virtual async Task<int> UpdateAsync(IEnumerable<Download> entities, bool runInTransaction = true)
        {
            return await AsyncDbConnection.UpdateAllAsync(entities, runInTransaction);
        }
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await AsyncDbConnection.GetAsync<TEntity>(id);
        }
        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            return await AsyncDbConnection.Table<TEntity>().ToListAsync();
        }
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
