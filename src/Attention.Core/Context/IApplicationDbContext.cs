using SQLite;

namespace Attention.Core.Context
{
    public interface IApplicationDbContext
    {
        SQLiteConnection Conn { get; }
    }
}
