using HENG.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.Views
{
    /// <summary>
    /// http://meanme.com/2017/07/11/sticky-header/
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;
        public ShellPage()
        {
            this.InitializeComponent();
            ViewModel.Initialize();
        }
    }
}
