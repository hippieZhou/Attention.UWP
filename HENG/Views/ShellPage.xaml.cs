using Windows.UI.Xaml.Navigation;
using HENG.ViewModels;

namespace HENG.Views
{
    public sealed partial class ShellPage
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;

        public ShellPage()
        {
            InitializeComponent();
            ViewModel.Initialize(NavView, ContentFrame, DetailView, NotifGrid);
        }


        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Cleanup();
            base.OnNavigatingFrom(e);
        }
    }
}
