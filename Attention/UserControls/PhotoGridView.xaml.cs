using Attention.Extensions;
using Attention.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Attention.UserControls
{
    public sealed partial class PhotoGridView : UserControl
    {
        public PhotoGridView()
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
            DependencyProperty.Register("ViewModel", typeof(PhotoGridViewModel), typeof(PhotoGridView), new PropertyMetadata(null));


        private void AdaptiveGridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is AdaptiveGridView gridView && gridView.SelectedItem != null)
            {
                gridView.ScrollIntoView(gridView.SelectedItem);
            }
        }

        private void Grid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Grid container)
            {
                if (container.FindName("imageEx") is ImageEx imageEx)
                {
                    container.PlayScaleAnimation(imageEx, AnimationExtension.CreateScaleAnimation(true));
                }
            }
        }

        private void Grid_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Grid container)
            {
                if (container.FindName("imageEx") is ImageEx imageEx)
                {
                    container.PlayScaleAnimation(imageEx, AnimationExtension.CreateScaleAnimation(false));
                }
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
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
