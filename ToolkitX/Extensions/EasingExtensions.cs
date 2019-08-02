using System.Numerics;
using Windows.UI.Composition;

namespace ToolkitX.Extensions
{
    public static class EasingExtensions
    {
        public static CubicBezierEasingFunction EaseInOutCubic(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.645f, 0.045f), new Vector2(0.355f, 1.0f));
        }
    }
}
