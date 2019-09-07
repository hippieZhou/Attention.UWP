using Attention.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Attention
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;
        public ShellPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}
