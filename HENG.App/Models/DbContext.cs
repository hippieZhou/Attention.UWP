using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using Windows.Storage;

namespace HENG.App.Models
{
    public class DbContext
    {
        private SQLiteConnection DbConnection => new SQLiteConnection(new SQLitePlatformWinRT(), AppSettings.Current.DbPath);

        public DbContext(CreateFlags createFlags = CreateFlags.None)
        {
            using (SQLiteConnection db = DbConnection)
            {
                db.TraceListener = new DebugTraceListener();
                db.CreateTable<DownloadItem>(createFlags);
            }
        }

        public int AddDownloadItem(DownloadItem item)
        {
            using (var db = DbConnection)
            {
                db.TraceListener = new DebugTraceListener();
                return db.InsertOrIgnore(item, typeof(DownloadItem));
            }
        }

        public int DelDownloadItem(DownloadItem item)
        {
            using (var db = DbConnection)
            {
                db.TraceListener = new DebugTraceListener();
                return db.Delete<DownloadItem>(item.Id);
                //SQLiteCommand cmd = db.CreateCommand($"DELETE FROM {nameof(DownloadItem)} WHERE Id = @id", item.Id);
                //return cmd.ExecuteNonQuery();
            }
        }

        public DownloadItem FindDownloadItem(int id)
        {
            using (var db = DbConnection)
            {
                db.TraceListener = new DebugTraceListener();
                return db.Find<DownloadItem>(id);
            }
        }

        public IEnumerable<DownloadItem> GetAllDownloads()
        {
            List<DownloadItem> downloads = new List<DownloadItem>();

            using (var db = DbConnection)
            {
                db.TraceListener = new DebugTraceListener();
                downloads.AddRange(from p in db.Table<DownloadItem>() select p);
            }

            return downloads;
        }
    }

    public class DebugTraceListener : ITraceListener
    {
        public void Receive(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
