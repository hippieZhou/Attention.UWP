using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using System.Numerics;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace HENG.UserControls
{
    public sealed partial class PhotoControl : UserControl
    {
        public PhotoControl()
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
            DependencyProperty.Register("Photos", typeof(object), typeof(PhotoControl), new PropertyMetadata(null));

        public Visibility LoadingVisibility
        {
            get { return (Visibility)GetValue(LoadingVisibilityProperty); }
            set { SetValue(LoadingVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadingVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingVisibilityProperty =
            DependencyProperty.Register("LoadingVisibility", typeof(Visibility), typeof(PhotoControl), new PropertyMetadata(Visibility.Visible));

        public Visibility ErrorVisibility
        {
            get { return (Visibility)GetValue(ErrorVisibilityProperty); }
            set { SetValue(ErrorVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorVisibilityProperty =
            DependencyProperty.Register("ErrorVisibility", typeof(Visibility), typeof(PhotoControl), new PropertyMetadata(Visibility.Collapsed));

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RefreshCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(PhotoControl), new PropertyMetadata(null));

        public ICommand DownloadCommand
        {
            get { return (ICommand)GetValue(DownloadCommandProperty); }
            set { SetValue(DownloadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DownloadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DownloadCommandProperty =
            DependencyProperty.Register("DownloadCommand", typeof(ICommand), typeof(PhotoControl), new PropertyMetadata(null));

        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemClickCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(PhotoControl), new PropertyMetadata(null));

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
            imgVisual.StartAnimation("Scale.X", scaleAnimation);
            imgVisual.StartAnimation("Scale.Y", scaleAnimation);
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

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                DownloadCommand?.Execute(btn.CommandParameter);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Implicit.GetAnimations(refresh)?.StartAnimation(refresh);
        }

        private void AdaptiveGridViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer viewer = AdaptiveGridViewControl.FindDescendant<ScrollViewer>();
            if (viewer != null)
            {
                viewer.ViewChanging += (s1, e1) => 
                {
                    if (refresh.Visibility == Visibility.Visible)
                    {
                        refresh.Visibility = Visibility.Collapsed;
                    }
                };
                viewer.ViewChanged += (s2, e2) => 
                {
                    refresh.Visibility = Visibility.Visible;
                    Implicit.GetShowAnimations(refresh)?.StartAnimation(refresh);
                };
            }
        }
    }
}
