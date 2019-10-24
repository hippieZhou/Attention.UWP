using Attention.UWP.ViewModels;
using Windows.UI.Xaml.Controls;


namespace Attention.UWP.Views
{
    public sealed partial class MainView : UserControl
    {
        public MainViewModel ViewModel => ViewModelLocator.Current.Main;
        public MainView()
        {
            this.InitializeComponent();
        }
    }
}
