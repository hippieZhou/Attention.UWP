using Attention.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTime _dateTime;
        public string DbFilePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "default.db");

        public DbSet<TEntity> DbSet<TEntity>() where TEntity : AuditableEntity => Set<TEntity>();

        public ApplicationDbContext(IDateTime dateTime)
        {
            _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbFilePath}");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
