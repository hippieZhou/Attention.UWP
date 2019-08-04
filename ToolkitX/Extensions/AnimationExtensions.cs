using System;
using Windows.UI.Composition;

namespace ToolkitX.Extensions
{
    public static class AnimationExtensions
    {
        public static ScalarKeyFrameAnimation CreateScalarKeyFrameAnimation(this Compositor compositor, float? from, float to,
            double duration, double delay, CompositionEasingFunction easing, AnimationIterationBehavior iterationBehavior)
        {
            var animation = compositor.CreateScalarKeyFrameAnimation();

            animation.Duration = TimeSpan.FromMilliseconds(duration);
            if (!delay.Equals(0)) animation.DelayTime = TimeSpan.FromMilliseconds(delay);
            if (from.HasValue) animation.InsertKeyFrame(0.0f, from.Value, easing);
            animation.InsertKeyFrame(1.0f, to, easing);
            animation.IterationBehavior = iterationBehavior;

            return animation;
        }
    }
}