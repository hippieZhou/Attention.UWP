using Attention.UWP.Extensions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Xaml.Interactivity;
using System;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace Attention.UWP.Behaviors
{
    public class GridScaleBehavior : Behavior
    {
        private Grid uiContainer;

        protected override void OnAttached()
        {
            base.OnAttached();

            this.uiContainer = this.AssociatedObject as Grid;
            if (this.uiContainer == null)
            {
                throw new InvalidOperationException("ImageExScaleBehavior can only be attached to types inheriting UIElement");
            }

            this.uiContainer.PointerEntered += UiElement_PointerEntered;
            this.uiContainer.PointerExited += UiElement_PointerExited;
            this.uiContainer.SizeChanged += UiContainer_SizeChanged;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.uiContainer != null)
            {
                this.uiContainer.PointerEntered -= UiElement_PointerEntered;
                this.uiContainer.PointerExited -= UiElement_PointerExited;
                this.uiContainer.SizeChanged += UiContainer_SizeChanged;
            }
        }

        private void UiElement_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (this.uiContainer.FindName("imageEx") is ImageEx imageEx)
            {
                this.uiContainer.PlayScaleAnimation(imageEx, AnimationExtension.CreateScaleAnimation(false));
            }
        }

        private void UiElement_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (this.uiContainer.FindName("imageEx") is ImageEx imageEx)
            {
                this.uiContainer.PlayScaleAnimation(imageEx, AnimationExtension.CreateScaleAnimation(true));
            }
        }

        private void UiContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize == e.PreviousSize) return;
            if (sender is Grid container)
            {
                container.Clip = new RectangleGeometry()
                {
                    Rect = new Rect(0, 0, container.ActualWidth, container.ActualHeight)
                };
            }
        }
    }
}
