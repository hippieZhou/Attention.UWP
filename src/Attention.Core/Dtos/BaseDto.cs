using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Attention.Core.Dtos
{
    public abstract class BaseDto<T> where T : class, new()
    {
        public static IEnumerable<T> FakeData { get; protected set; }
        protected readonly static Random random = new Random(DateTime.Now.Second);
        protected readonly static Color[] colors = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null)).ToArray();
        protected static Brush CreateAcrylicBrush(Color color) => new AcrylicBrush
        {
            BackgroundSource = AcrylicBackgroundSource.Backdrop,
            TintColor = color,
            FallbackColor = color,
            TintOpacity = 1.0,
            TintLuminosityOpacity = 1.0,
        };
    }
}
