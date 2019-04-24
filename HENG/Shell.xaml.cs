using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using HENG.ViewModel;

namespace HENG
{
    public sealed partial class Shell
    {
        public ShellViewModel ViewModel => (ShellViewModel)DataContext;

        public Shell()
        {
            InitializeComponent();
            ViewModel.Initialize(ContentFrame, NavView);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Cleanup();
            base.OnNavigatingFrom(e);
        }
    }
}
