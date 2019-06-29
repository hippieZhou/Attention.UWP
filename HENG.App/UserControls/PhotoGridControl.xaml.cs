using HENG.App.Helpers;
using HENG.App.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace HENG.App.UserControls
{
    public sealed partial class PhotoGridControl : UserControl
    {
        public PhotoGridControl()
        {
            this.InitializeComponent();
        }

        private void AdaptiveGridViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer viewer = adaptiveGridViewControl.GetFirstDescendantOfType<ScrollViewer>();

            Action animation = new Action(async () =>
            {
                bool isShow = false;

                if (viewer.VerticalOffset > 0 && !isShow)
                {
                    await backToTopBtn.Offset(0, 0, duration: 800, delay: 200).StartAsync();
                    isShow = true;
                }
                else
                {
                    await backToTopBtn.Offset(100.0f, 0, duration: 800, delay: 600).StartAsync();
                    isShow = false;
                }
            });

            if (viewer != null)
            {
                viewer.ViewChanged += (s1, e1) =>
                {
                    animation();
                };

                viewer.SizeChanged += (s2, e2) =>
                {
                    animation();
                };
            }

            animation();

            ParallaxingAnimation(viewer);
        }

        private ExpressionNode _parallaxExpression;

        private void ParallaxingAnimation(ScrollViewer viewer)
        {
            CompositionPropertySet scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(viewer);

            var scrollPropSet = scrollProperties.GetSpecializedReference<ManipulationPropertySetReferenceNode>();
            var startOffset = ExpressionValues.Constant.CreateConstantScalar("startOffset", 0.0f);
            var parallaxValue = 0.5f;
            var parallax = (scrollPropSet.Translation.Y + startOffset);
            _parallaxExpression = parallax * parallaxValue - parallax;
        }

        public PhotoGridViewModel ViewModel
        {
            get { return (PhotoGridViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PhotoGridViewModel), typeof(PhotoGridControl), new PropertyMetadata(null));

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

        private void AdaptiveGridViewControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var item = adaptiveGridViewControl.SelectedItem;
            if (item != null)
            {
                adaptiveGridViewControl.ScrollIntoView(item);
            }
        }

        private void AdaptiveGridViewControl_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            //if (_parallaxExpression != null)
            //{
            //    ImageEx image = args.ItemContainer.ContentTemplateRoot.GetFirstDescendantOfType<ImageEx>();
            //    Visual visual = ElementCompositionPreview.GetElementVisual(image);
            //    visual.Size = new Vector2(960f, 960f);
            //    _parallaxExpression.SetScalarParameter("StartOffset", (float)args.ItemIndex * visual.Size.Y / 4.0f);
            //    visual.StartAnimation("Offset.Y", _parallaxExpression);
            //}
        }
    }
}
