using HENG.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HENG.Views
{
    public sealed partial class LocalView : Page
    {
        public LocalViewModel ViewModel => ViewModelLocator.Current.Local;

        public LocalView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        private void Grid_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
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
