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
    }
}
