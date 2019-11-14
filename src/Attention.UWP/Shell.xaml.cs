using Attention.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP
{
    public sealed partial class Shell : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;

        public Shell()
        {
            this.InitializeComponent();
            ViewModel.Initialize(mainView);
            DataContext = ViewModel;
        }
    }
}
