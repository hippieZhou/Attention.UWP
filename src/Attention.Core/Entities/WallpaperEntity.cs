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
        [Column("Width"), NotNull]
        public int Width { get; set; }
        [Column("Height"), NotNull]
        public int Height { get; set; }

        [Column("Description")]
        public string Description { get; set; }
    }
}
