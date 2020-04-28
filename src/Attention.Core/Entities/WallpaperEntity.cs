using SQLite;

namespace Attention.Core.Entities
{
    [Table("wallpaper_info")]
    public class WallpaperEntity : AuditableEntity
    {
        [Column("background"), NotNull]
        public string Background { get; set; }
        [Column("thumbnail"), NotNull]
        public string Thumbnail { get; set; }
    }
}
