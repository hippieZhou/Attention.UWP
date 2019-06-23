using HENG.App.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.App.Views
{
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel => ViewModelLocator.Current.Home;
        public HomePage()
        {
            this.InitializeComponent();
            ViewModel.Initialize(masterView.FindName("adaptiveGridViewControl") as GridView, detailView.FindName("SmokeGrid") as Grid);
        }
    }
}
