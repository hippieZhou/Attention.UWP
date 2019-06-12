using HENG.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG
{
    public sealed partial class Shell : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;

        public Shell()
        {
            this.InitializeComponent();
            ViewModel.Initialize(NavView, ContentFrame, PhotoInfo.FindName("SmokeGrid") as Grid);
        }
    }
}
