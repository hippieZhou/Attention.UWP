using Attention.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.Views
{
    public sealed partial class LocalView : UserControl
    {
        public LocalViewModel ViewModel => ViewModelLocator.Current.Local;
        public LocalView()
        {
            this.InitializeComponent();
        }

        private void FlipPanel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is FlipPanel handler)
            {
                handler.IsFlipped = !handler.IsFlipped;
            }
        }
    }
}
