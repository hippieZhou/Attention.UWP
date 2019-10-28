using Attention.UWP.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;

        public ShellPage()
        {
            this.InitializeComponent();
            ViewModel.Initialize(mainView);
            this.DataContext = ViewModel;
            Messenger.Default.Register<string>(this, nameof(InAppNotification), str =>
            {
                InAppNotification.Show(str, 2000);
            });
        }
    }
}
