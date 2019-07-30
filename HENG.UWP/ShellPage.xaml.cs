using HENG.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.UWP
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;
        public ShellPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView);
        }
    }
}
