using PixabaySharp.Models;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace HENG.App.Models
{
    public sealed class DbContext
    {
        private string dbPath = string.Empty;
        private string DbPath
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

        private SQLiteConnection DbConnection => new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);

        public DbContext()
        {
            using (SQLiteConnection db = DbConnection)
            {
                db.TraceListener = new DebugTraceListener();
                db.CreateTable<ImageItem>(CreateFlags.None);
            }
        }

        public IEnumerable<T> GetAllItems<T>() where T : class
        {
            List<T> models = new List<T>();
            using (SQLiteConnection db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.TraceListener = new DebugTraceListener();
                models.AddRange(from p in db.Table<T>() select p);
            }
            return models;
        }

        public int InsertItem(ImageItem item)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.TraceListener = new DebugTraceListener();
                int count = (from p in db.Table<ImageItem>()
                             where p.Id == item.Id
                             select p).Count();
                return count == 0 ? db.Insert(item) : count;
            }
        }

        public int DeleteItem(ImageItem item)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                db.TraceListener = new DebugTraceListener();
                SQLiteCommand cmd = db.CreateCommand($"DELETE FROM {nameof(ImageItem)} WHERE Id = @id", item.Id);
                return cmd.ExecuteNonQuery();
            }
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
