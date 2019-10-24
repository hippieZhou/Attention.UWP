using Attention.UWP.Models;
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
                var c = db.CreateTable<DownloadItem>();
                var info = db.GetMapping(typeof(DownloadItem));
            }
        }

        public static async Task DeleteDownloadAsync(DownloadItem download)
        {
            await AsyncDbConnection.ExecuteAsync($"DELETE FROM {nameof(DownloadItem)} WHERE Id = ?", download.Id);
        }

        public static async Task<IEnumerable<DownloadItem>> GetAllDownloadsAsync()
        {
            return await AsyncDbConnection.Table<DownloadItem>().ToListAsync();
        }

        public static async Task<DownloadItem> GetDownloadByIdAsync(int Id)
        {
            return await AsyncDbConnection.Table<DownloadItem>().FirstOrDefaultAsync(p => p.Id == Id);
        }

        public static async Task<int> SaveDownloadAsync(DownloadItem download)
        {
            return download.Id == 0 ?
                await AsyncDbConnection.InsertAsync(download) :
                await AsyncDbConnection.UpdateAsync(download);
        }
    }
}
