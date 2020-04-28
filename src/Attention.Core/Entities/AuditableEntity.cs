using SQLite;
using System;

namespace Attention.Core.Entities
{
    public class AuditableEntity
    {
        [Column("id"), PrimaryKey, AutoIncrement]
        public Guid Id { get; set; }

        [Column("created"), NotNull]
        public DateTime Created { get; set; }

        [Column("lastModified")]
        public DateTime? LastModified { get; set; }
    }
}
