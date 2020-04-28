using System;
using System.Runtime.CompilerServices;

namespace Attention.Framework
{
    public class EnginContext
    {
        public static IEngine Current { get; private set; }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Initialize(IEngine engine)
        {
            Current = engine ?? throw new ArgumentNullException(nameof(engine));
        }
    }
}
