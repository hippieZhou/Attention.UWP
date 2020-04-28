using System;

namespace Attention.Context
{
    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
