using Attention.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.Views
{
    public sealed partial class MoreView : UserControl
    {
        public MoreViewModel ViewModel => ViewModelLocator.Current.More;
        public MoreView()
        {
            this.InitializeComponent();
        }

        private void StackPanel_PreviewKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {

        }
    }
}
