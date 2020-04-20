using Attention.App.Extensions;
using Microsoft.Xaml.Interactivity;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Attention.App.Behaviors
{
    public class ElementScaleAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var root = TargetObject ?? sender as FrameworkElement;

            if (root is null)
            {
                throw new InvalidOperationException("ElementScalarAction can only be attached to types inheriting UIElement");
            }

            if (VisualTreeHelper.GetParent(root) is FrameworkElement parent)
            {
                parent.Clip = new RectangleGeometry()
                {
                    Rect = new Rect(0, 0, parent.ActualWidth, parent.ActualHeight)
                };
            }
            return root.Play(root.CreateScaleAnimation(Entered));
        }

        public bool Entered
        {
            get { return (bool)GetValue(EnteredProperty); }
            set { SetValue(EnteredProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Enable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnteredProperty =
            DependencyProperty.Register("Entered", typeof(bool), typeof(ElementScaleAction), new PropertyMetadata(false));

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
