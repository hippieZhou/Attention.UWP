using SQLite;

namespace Attention.Core.Context
{
    public class ApplicationDbContext: IApplicationDbContext
    {
        private readonly string _dbFile;
        public SQLiteConnection Conn => new SQLiteConnection(_dbFile);
        public ApplicationDbContext(string dbFile) => _dbFile = dbFile;
    }
}
