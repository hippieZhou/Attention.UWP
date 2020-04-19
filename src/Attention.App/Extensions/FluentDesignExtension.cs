using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Attention.App.Extensions
{
    public static class FluentDesignExtension
    {
        public static Visual ElementVisual(this UIElement element) => ElementCompositionPreview.GetElementVisual(element);

        private const float SCALE_ANIMATION_FACTOR = 1.05f;

        public static Vector3KeyFrameAnimation CreateScaleAnimation(this UIElement root,bool show)
        {
            var rootVisual = root.ElementVisual();
            var scaleAnimation = rootVisual.Compositor.CreateVector3KeyFrameAnimation();
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            scaleAnimation.InsertKeyFrame(1.0f, new Vector3(show ? SCALE_ANIMATION_FACTOR : 1.0f));
            return scaleAnimation;
        }

        public static FrameworkElement Play(this FrameworkElement root, Vector3KeyFrameAnimation scaleAnimation)
        {
            var rootVisual = root.ElementVisual();
            if (rootVisual.CenterPoint.X == 0 && rootVisual.CenterPoint.Y == 0)
            {
                rootVisual.CenterPoint = new Vector3((float)(root.ActualWidth / 2.0), (float)(root.ActualHeight / 2.0), 0f);
            }
            rootVisual.StartAnimation("Scale", scaleAnimation);
            return root;
        }
    }
}
