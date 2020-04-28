using System;

namespace Attention.Core.Entities
{
    public class AuditableEntity
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
