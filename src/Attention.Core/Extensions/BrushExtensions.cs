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

        public static Brush ToBrush(this string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)Convert.ToUInt32(hex.Substring(0, 2), 16);
            byte r = (byte)Convert.ToUInt32(hex.Substring(2, 2), 16);
            byte g = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);
            byte b = (byte)Convert.ToUInt32(hex.Substring(6, 2), 16);
            return new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
        }
    }
}
