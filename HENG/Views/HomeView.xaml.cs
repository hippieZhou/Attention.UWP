using HENG.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HENG.Views
{
    public sealed partial class HomeView : Page
    {
        public HomeViewModel ViewModel => ViewModelLocator.Current.Home;
        public HomeView()
        {
            this.InitializeComponent();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}
