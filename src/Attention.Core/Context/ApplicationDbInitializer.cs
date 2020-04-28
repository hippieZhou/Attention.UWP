using Attention.Core.Entities;
using SQLite.Net.Interop;

namespace Attention.Core.Context
{
    public static class ApplicationDbInitializer
    {
        public static void Migrate(IApplicationDbContext dbContext)
        {
            using (var conn = dbContext.Conn)
            {
                conn.CreateTable<WallpaperEntity>(CreateFlags.None);
                conn.Commit();
            }
        }
    }
}
