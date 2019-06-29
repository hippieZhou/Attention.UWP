using HENG.App.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HENG.App.UserControls
{
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
            DependencyProperty.Register("ViewModel", typeof(PhotoGridViewModel), typeof(PhotoGridControl), new PropertyMetadata(null, (d, e) => 
            {
                if (d is PhotoDetailControl handler)
                {
                    handler.DataContext = e.NewValue;
                }
            }));


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
