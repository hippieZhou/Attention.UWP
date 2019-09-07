using HENG.App.ViewModels;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HENG.App.Views
{
    public sealed partial class LocalView : UserControl
    {
        public LocalViewModel ViewModel => ViewModelLocator.Current.Local;
        public LocalView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;

            //this.Loaded += (sender, e) =>
            //{
            //    var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            //    _visual = compositor.CreateSpriteVisual();
            //    _visual.Offset = new Vector3(50.0f, 50.0f, 0.0f);

            //    var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
            //    offsetAnimation.Target = nameof(Visual.Offset);
            //    offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
            //    offsetAnimation.Duration = TimeSpan.FromMilliseconds(1500);

            //    var rotationAnimation = compositor.CreateScalarKeyFrameAnimation();
            //    rotationAnimation.Target = nameof(Visual.RotationAngleInDegrees);
            //    rotationAnimation.InsertKeyFrame(0.0f, 0.0f);
            //    rotationAnimation.InsertKeyFrame(1.0f, 360.0f);
            //    rotationAnimation.Duration = TimeSpan.FromMilliseconds(1500);

            //    var animationGroup = compositor.CreateAnimationGroup();
            //    animationGroup.Add(offsetAnimation);
            //    animationGroup.Add(rotationAnimation);

            //    var implicitAnimations = compositor.CreateImplicitAnimationCollection();
            //    implicitAnimations[nameof(Visual.Offset)] = animationGroup;

            //    _visual.ImplicitAnimations = implicitAnimations;

            //    ElementCompositionPreview.SetElementChildVisual(backToUpBtn, _visual);
            //};
        }

        public ICommand NavToBackCommand
        {
            get { return (ICommand)GetValue(NavToBackCommandProperty); }
            set { SetValue(NavToBackCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavToNackCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavToBackCommandProperty =
            DependencyProperty.Register("NavToNackCommand", typeof(ICommand), typeof(LocalView), new PropertyMetadata(null));

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize == e.PreviousSize) return;

            var rootGrid = sender as Grid;
            rootGrid.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, rootGrid.ActualWidth, rootGrid.ActualHeight)
            };
        }
    }
}
