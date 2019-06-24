using HENG.App.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.App
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;
        public ShellPage()
        {
            this.InitializeComponent();
            ViewModel.Initialize(this.titleBar.FindName("backButton") as Button, this.titleBar.FindName("menuButton") as Button);
            this.DataContext = ViewModel;
        }
    }
}
