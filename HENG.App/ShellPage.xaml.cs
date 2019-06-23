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
            ViewModel.Initialize(contentFrame);
            this.DataContext = ViewModel;
        }
    }
}
