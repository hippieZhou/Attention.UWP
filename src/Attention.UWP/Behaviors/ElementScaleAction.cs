using Attention.UWP.Extensions;
using Microsoft.Xaml.Interactivity;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Attention.UWP.Behaviors
{
    public class ElementScaleAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var uiElement = TargetObject ?? sender as FrameworkElement;

            if(uiElement is null)
            {
                throw new InvalidOperationException("ElementScalarAction can only be attached to types inheriting UIElement");
            }

            if (VisualTreeHelper.GetParent(uiElement) is FrameworkElement parent)
            {
                parent.SizeChanged += (_sender, e) =>
                {
                    if (e.NewSize == e.PreviousSize)
                        return;

                    parent.Clip = new RectangleGeometry()
                    {
                        Rect = new Rect(0, 0, parent.ActualWidth, parent.ActualHeight)
                    };
                };
            }

            uiElement.PlayScaleAnimation(uiElement, AnimationExtension.CreateScaleAnimation(Enable));
            return null;
        }

        public bool Enable
        {
            get { return (bool)GetValue(EnableProperty); }
            set { SetValue(EnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Enable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.Register("Enable", typeof(bool), typeof(ElementScaleAction), new PropertyMetadata(false));

        public FrameworkElement TargetObject
        {
            get { return (FrameworkElement)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(FrameworkElement), typeof(ElementScaleAction), new PropertyMetadata(null));
    }
}
