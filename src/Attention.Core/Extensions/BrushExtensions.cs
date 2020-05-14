using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Media;

namespace Attention.Core.Extensions
{
    public static class BrushExtensions
    {
        private static readonly Random _random = new Random();
        public static Brush Random(this IEnumerable<Brush> brushs)
        {
            var index = _random.Next(0, brushs.Count());
            return brushs.ElementAt(index);
        }
    }
}
