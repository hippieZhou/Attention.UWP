using Windows.UI.Xaml.Navigation;
using HENG.ViewModels;
using Windows.UI.Xaml;

namespace HENG.Views
{
    public sealed partial class ShellPage
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;

        public ShellPage()
        {
            InitializeComponent();
            ViewModel.Initialize(ContentFrame, NavView, DetailView, NotifGrid);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Cleanup();
            base.OnNavigatingFrom(e);
        }
    }
}
