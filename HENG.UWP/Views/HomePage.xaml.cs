using HENG.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.UWP.Views
{
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel => ViewModelLocator.Current.Home;

        public HomePage()
        {
            this.InitializeComponent();
        }
    }
}
