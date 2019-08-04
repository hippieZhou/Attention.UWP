using System;
using Windows.UI.Composition;

namespace ToolkitX.Extensions
{
    public static partial class CompositionExtensions
    {
        public static void StartOpacityAnimation(this Visual visual, float? from = null, float to = 1.0f,
            double duration = 800, int delay = 0, CompositionEasingFunction easing = null, Action completed = null,
            AnimationIterationBehavior iterationBehavior = AnimationIterationBehavior.Count)
        {
            CompositionScopedBatch batch = null;
            var compositor = visual.Compositor;

            if (completed != null)
            {
                batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                batch.Completed += (s, e) => completed();
            }

            visual.StartAnimation("Opacity",
                compositor.CreateScalarKeyFrameAnimation(from, to, duration, delay, easing, iterationBehavior));

            batch?.End();
        }
    }
}