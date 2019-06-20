using HENG.ViewModels;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HENG.UserControls
{
    public sealed partial class LocalControl : UserControl
    {
        public LocalViewModel ViewModel => ViewModelLocator.Current.Local;
        public ICommand BackCommand => ViewModelLocator.Current.Shell.BackCommand;

        public LocalControl()
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
