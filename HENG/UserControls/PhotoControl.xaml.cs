using HENG.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace HENG.UserControls
{
    public sealed partial class PhotoControl : UserControl
    {
        public PhotoViewModel ViewModel => ViewModelLocator.Current.Photo;
        private ScrollViewer myScrollViewer;
        public PhotoControl()
        {
            this.InitializeComponent();
            ViewModel.Initialize(FindName("AdaptiveGridViewControl") as GridView);

            AdaptiveGridViewControl.Loaded += (sender, e) =>
            {
                myScrollViewer = AdaptiveGridViewControl.FindDescendant<ScrollViewer>();
            };
        }

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(PhotoControl), new PropertyMetadata(null));

        private void AdaptiveGridViewControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var item = AdaptiveGridViewControl.SelectedItem;
            if (item != null)
            {
                AdaptiveGridViewControl.ScrollIntoView(item);
            }
        }

        private void Grid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Grid root)
            {
                if (root.FindName("imageEx") is ImageEx imageEx)
                {
                    var scaleAnimation = CreateScaleAnimation(imageEx, true);
                    PlayScaleAnimation(root, imageEx, scaleAnimation);
                }
            }
        }

        private void Grid_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Grid root)
            {
                if (root.FindName("imageEx") is ImageEx imageEx)
                {
                    var scaleAnimation = CreateScaleAnimation(imageEx, false);
                    PlayScaleAnimation(root, imageEx, scaleAnimation);
                }
            }
        }

        private const float SCALE_ANIMATION_FACTOR = 1.05f;
        private ScalarKeyFrameAnimation CreateScaleAnimation(ImageEx imageEx, bool show)
        {
            var scaleAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(1f, show ? SCALE_ANIMATION_FACTOR : 1f);
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            scaleAnimation.StopBehavior = AnimationStopBehavior.LeaveCurrentValue;
            return scaleAnimation;
        }

        private void PlayScaleAnimation(Grid parent, ImageEx child, ScalarKeyFrameAnimation scaleAnimation)
        {
            var imgVisual = ElementCompositionPreview.GetElementVisual(child);
            if (imgVisual.CenterPoint.X == 0 && imgVisual.CenterPoint.Y == 0)
            {
                imgVisual.CenterPoint = new Vector3((float)parent.ActualWidth / 2, (float)parent.ActualHeight / 2, 0f);
            }
            imgVisual.StartAnimation("Scale.X", scaleAnimation);
            imgVisual.StartAnimation("Scale.Y", scaleAnimation);
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize == e.PreviousSize) return;

            var rootGrid = sender as Grid;
            rootGrid.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, rootGrid.ActualWidth, rootGrid.ActualHeight)
            };
        }

        private void ToTopButton_Click(object sender, RoutedEventArgs e)
        {
            myScrollViewer?.ChangeView(0.0f, 0.0f, 1.0f, false);
        }
    }
}
