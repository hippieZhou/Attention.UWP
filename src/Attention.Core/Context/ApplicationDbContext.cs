using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace Attention.Core.Context
{
    public class ApplicationDbContext: IApplicationDbContext
    {
        private readonly string _dbFile;
        public SQLiteConnection Conn => new SQLiteConnection(new SQLitePlatformWinRT(), _dbFile);
        public ApplicationDbContext(string dbFile) => _dbFile = dbFile;
    }
}
