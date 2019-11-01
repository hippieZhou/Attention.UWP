using Attention.UWP.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
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
            DataContext = ViewModel;
        }
    }
}
