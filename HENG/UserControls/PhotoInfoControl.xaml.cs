using HENG.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HENG.UserControls
{
    public sealed partial class PhotoInfoControl : UserControl
    {
        public PhotoInfoControl()
        {
            this.InitializeComponent();
        }

        public PhotoInfoViewModel ViewModel
        {
            get { return (PhotoInfoViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PhotoInfoViewModel), typeof(PhotoInfoControl), new PropertyMetadata(null, (d, e) =>
            {
                if (d is PhotoInfoControl handler)
                {
                    PhotoInfoViewModel vm = e.NewValue as PhotoInfoViewModel;
                    vm.Initialize(handler.FindName("SmokeGrid") as Grid);
                    handler.DataContext = vm;
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
