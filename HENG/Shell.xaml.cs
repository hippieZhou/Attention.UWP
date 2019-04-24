using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using HENG.ViewModel;

namespace HENG
{
    public sealed partial class Shell
    {
        public ShellViewModel Vm => (ShellViewModel)DataContext;

        public Shell()
        {
            InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManagerBackRequested;

            Loaded += async (s, e) =>
            {
                await Vm.Initialize();
            };
        }

        private void SystemNavigationManagerBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Vm.Cleanup();
            base.OnNavigatingFrom(e);
        }
    }
}
