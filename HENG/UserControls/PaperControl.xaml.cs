using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using System.Numerics;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace HENG.UserControls
{
    public sealed partial class PaperControl : UserControl
    {
        public PaperControl()
        {
            this.InitializeComponent();
        }

        public object Photos
        {
            get { return (object)GetValue(PhotosProperty); }
            set { SetValue(PhotosProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Photos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PhotosProperty =
            DependencyProperty.Register("Photos", typeof(object), typeof(PaperControl), new PropertyMetadata(null));


        public Visibility FooterVisibility
        {
            get { return (Visibility)GetValue(FooterVisibilityProperty); }
            set { SetValue(FooterVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FooterVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterVisibilityProperty =
            DependencyProperty.Register("FooterVisibility", typeof(Visibility), typeof(PaperControl), new PropertyMetadata(null));



        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RefreshCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(PaperControl), new PropertyMetadata(null));

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

        private void PlayScaleAnimation(Grid parent, ImageEx child, ScalarKeyFrameAnimation scaleAnimation)
        {
            var imgVisual = ElementCompositionPreview.GetElementVisual(child);
            if (imgVisual.CenterPoint.X == 0 && imgVisual.CenterPoint.Y == 0)
            {
                imgVisual.CenterPoint = new Vector3((float)parent.ActualWidth / 2, (float)parent.ActualHeight / 2, 0f);
            }
            imgVisual.StartAnimation("Scale.x", scaleAnimation);
            imgVisual.StartAnimation("Scale.y", scaleAnimation);
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

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var rootGrid = sender as Grid;
            rootGrid.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, rootGrid.ActualWidth, rootGrid.ActualHeight)
            };
        }
    }
}
