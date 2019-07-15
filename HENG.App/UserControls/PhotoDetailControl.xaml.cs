using HENG.App.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace HENG.App.UserControls
{
    /// <summary>
    /// https://www.arthurrump.com/2018/07/21/animating-gridviewitems-with-windows-ui-composition-aka-the-visual-layer/
    /// </summary>
    public sealed partial class PhotoDetailControl : UserControl
    {
        public PhotoDetailControl()
        {
            this.InitializeComponent();
        }

        public PhotoGridViewModel ViewModel
        {
            get { return (PhotoGridViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PhotoGridViewModel), typeof(PhotoMasterControl), new PropertyMetadata(null, (d, e) => 
            {
                if (d is PhotoDetailControl handler)
                {
                    handler.DataContext = e.NewValue;
                }
            }));

        private void DropShadowPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var root = (UIElement)sender;
            //root.GetFirstDescendantOfType<Canvas>()

            var rootVisual = ElementCompositionPreview.GetElementVisual(root);
            var compositor = rootVisual.Compositor;
            var pointerEnteredAnimation = compositor.CreateVector3KeyFrameAnimation();
            pointerEnteredAnimation.InsertKeyFrame(1.0f, new Vector3(1.1f));
            var pointerExitedAnimation = compositor.CreateVector3KeyFrameAnimation();
            pointerExitedAnimation.InsertKeyFrame(1.0f, new Vector3(1.0f));
            root.PointerEntered += (s, args) =>
            {
                rootVisual.CenterPoint = new Vector3(rootVisual.Size / 2, 0);
                rootVisual.StartAnimation("Scale", pointerEnteredAnimation);
            };
            root.PointerExited += (_, args) => rootVisual.StartAnimation("Scale", pointerExitedAnimation);
        }

        private void Grid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            infoGrid.Offset(
                offsetX: 0f,
                offsetY: 20f,
                duration: 250,
                delay: 75,
                easingMode: EasingMode.EaseInOut).Start();
        }

        private void Grid_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            infoGrid.Offset(
                offsetX: 0f,
                offsetY: 0f,
                duration: 250,
                delay: 75,
                easingMode: EasingMode.EaseInOut).Start();
        }
    }
}
