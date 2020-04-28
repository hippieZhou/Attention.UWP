using SQLite.Net;

namespace Attention.Core.Context
{
    public interface IApplicationDbContext
    {
        SQLiteConnection Conn { get; }
    }
}
