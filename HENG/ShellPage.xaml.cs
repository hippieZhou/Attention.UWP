using HENG.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;

        public ShellPage()
        {
            this.InitializeComponent();
            ViewModel.Initialize(NavView, ContentFrame);
        }
    }
}
