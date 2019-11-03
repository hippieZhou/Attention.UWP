using Attention.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;

        public ShellPage()
        {
            this.InitializeComponent();
            ViewModel.Initialize(mainView ?? FindName("mainView"));
            DataContext = ViewModel;
        }
    }
}
