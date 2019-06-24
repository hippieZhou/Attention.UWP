using HENG.App.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.App.Views
{
    public sealed partial class HomeView : UserControl
    {
        public HomeViewModel ViewModel => ViewModelLocator.Current.Home;
        public HomeView()
        {
            this.InitializeComponent();
            ViewModel.Initialize(
                masterView.FindName("adaptiveGridViewControl") as GridView,
                detailView.FindName("SmokeGrid") as Grid);
        }
    }
}
