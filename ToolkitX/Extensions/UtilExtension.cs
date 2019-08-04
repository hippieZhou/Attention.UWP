using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace ToolkitX.Extensions
{
    public static class UtilExtension
    {
        public static float ToFloat(this double value) => (float)value;

        public static List<FrameworkElement> Children(this DependencyObject parent)
        {
            var list = new List<FrameworkElement>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is FrameworkElement)
                {
                    list.Add(child as FrameworkElement);
                }

                list.AddRange(Children(child));
            }

            return list;
        }

        public static async Task ScrollToElementAsync(this ScrollViewer scrollViewer, FrameworkElement element,
            bool isVerticalScrolling = true, bool smoothScrolling = true, float? zoomFactor = null, bool bringToTopOrLeft = true)
        {
            if (!bringToTopOrLeft && element.IsFullyVisibile(scrollViewer))
                return;

            var contentArea = (FrameworkElement)scrollViewer.Content;
            var position = element.RelativePosition(contentArea);

            if (isVerticalScrolling)
            {
                await scrollViewer.ChangeViewAsync(null, position.Y, zoomFactor, !smoothScrolling);
            }
            else
            {
                await scrollViewer.ChangeViewAsync(position.X, null, zoomFactor, !smoothScrolling);
            }
        }

        public static bool IsFullyVisibile(this FrameworkElement element, FrameworkElement parent)
        {
            if (element == null || parent == null)
                return false;

            if (element.Visibility != Visibility.Visible)
                return false;

            var elementBounds = element.TransformToVisual(parent).TransformBounds(new Rect(0, 0, element.ActualWidth, element.ActualHeight));
            var containerBounds = new Rect(0, 0, parent.ActualWidth, parent.ActualHeight);

            var originalElementWidth = elementBounds.Width;
            var originalElementHeight = elementBounds.Height;

            elementBounds.Intersect(containerBounds);

            var newElementWidth = elementBounds.Width;
            var newElementHeight = elementBounds.Height;

            return originalElementWidth.Equals(newElementWidth) && originalElementHeight.Equals(newElementHeight);
        }

        public static Point RelativePosition(this UIElement element, UIElement other) => element.TransformToVisual(other).TransformPoint(new Point(0, 0));

        public static Task ChangeViewAsync(this ScrollViewer scrollViewer, double? horizontalOffset, double? verticalOffset, float? zoomFactor, bool disableAniamtion)
        {
            var taskSource = new TaskCompletionSource<bool>();

            void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
            {
                if (e.IsIntermediate) return;

                scrollViewer.ViewChanged -= OnViewChanged;
                taskSource.SetResult(true);
            }

            scrollViewer.ViewChanged += OnViewChanged;
            scrollViewer.ChangeView(horizontalOffset, verticalOffset, zoomFactor, disableAniamtion);

            return taskSource.Task;
        }

        public static CompositionPropertySet ScrollProperties(this ScrollViewer scrollViewer) => ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scrollViewer);

        public static void StartOffsetAnimation(this Visual visual, AnimationAxis axis, float? from = null, float to = 0,
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

            visual.StartAnimation($"Offset.{axis}", compositor.CreateScalarKeyFrameAnimation(from, to, duration, delay, easing, iterationBehavior));

            batch?.End();
        }

        public static void StartScaleAnimation(this Visual visual, AnimationAxis axis, float? from = null, float to = 0,
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

            visual.StartAnimation($"Scale.{axis}",
                compositor.CreateScalarKeyFrameAnimation(from, to, duration, delay, easing, iterationBehavior));

            batch?.End();
        }
    }
}