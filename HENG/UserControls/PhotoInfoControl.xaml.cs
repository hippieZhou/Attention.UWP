using HENG.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HENG.UserControls
{
    public sealed partial class PhotoInfoControl : UserControl
    {
        public PhotoInfoViewModel ViewModel => ViewModelLocator.Current.PhotoInfo;
        public PhotoInfoControl()
        {
            this.InitializeComponent();
            ViewModel.Initialize(FindName("SmokeGrid") as Grid);
            this.DataContext = ViewModel;
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
    }
}
