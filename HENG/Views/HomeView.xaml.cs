using HENG.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.Views
{
    public sealed partial class HomeView : UserControl
    {
        public HomeViewModel ViewModel => ViewModelLocator.Current.Home;
        public HomeView()
        {
            this.InitializeComponent();
        }
    }
}
