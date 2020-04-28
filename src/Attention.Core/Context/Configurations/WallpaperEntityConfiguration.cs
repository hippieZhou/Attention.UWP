using Attention.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attention.Core.Context.Configurations
{
    public class WallpaperEntityConfiguration : IEntityTypeConfiguration<WallpaperEntity>
    {
        public void Configure(EntityTypeBuilder<WallpaperEntity> builder)
        {
            builder.ToTable(nameof(WallpaperEntity));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Created).IsRequired();
        }
    }
}
