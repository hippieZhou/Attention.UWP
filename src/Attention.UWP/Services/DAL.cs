using Attention.UWP.Models.Core;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Attention.UWP.Services
{
    public static class DAL
    {
        private static string dbPath = string.Empty;
        public static string DbPath
        {
            get
            {
                if (string.IsNullOrEmpty(dbPath))
                {
                    dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite");
                }

                return dbPath;
            }
        }

        private static SQLiteAsyncConnection AsyncDbConnection
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

        /// <summary>
        /// https://github.com/XamlBrewer/UWP-SQLite-Sample
        /// https://github.com/praeclarum/sqlite-net
        /// </summary>
        static DAL()
        {

            using (var db = new SQLiteConnection(DbPath))
            {
                db.Tracer = str => Debug.WriteLine(str);

                // Create the table if it does not exist
                var c = db.CreateTable<Download>();
                var info = db.GetMapping(typeof(Download));
            }
        }

        public static async Task<int> DeleteAsync(Download item) => await DeleteAsync<Download>(item);

        public static async Task<IEnumerable<Download>> GetAllAsync() => await GetAllAsync<Download>();

        public static async Task<Download> GetByIdAsync(int Id) => await GetByIdAsync<Download>(Id);

        public static async Task<int> SaveOrUpdateAsync(Download item) => await SaveOrUpdateAsync<Download>(item);

        #region Inner Methods
        private static async Task<int> SaveOrUpdateAsync<T>(T item) where T : Entity
        {
            return item.Id == 0 ?
                await AsyncDbConnection.InsertAsync(item) :
                await AsyncDbConnection.UpdateAsync(item);
        }
        private static async Task<int> DeleteAsync<T>(T item) where T : Entity
        {
            return await AsyncDbConnection.ExecuteAsync($"DELETE FROM {typeof(T).Name} WHERE Id = ?", item.Id);
        }
        private static async Task<T> GetByIdAsync<T>(int Id) where T : Entity, new()
        {
            return await AsyncDbConnection.Table<T>().FirstOrDefaultAsync(p => p.Id == Id);
        }
        private static async Task<IEnumerable<T>> GetAllAsync<T>() where T : Entity, new()
        {
            return await AsyncDbConnection.Table<T>().ToListAsync();
        }
        #endregion
    }
}
