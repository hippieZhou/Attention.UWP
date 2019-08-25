using Attention.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.Views
{
    public sealed partial class DownloadView : UserControl
    {
        public DownloadView()
        {
            this.InitializeComponent();
        }

        public DownloadViewModel ViewModel
        {
            get { return (DownloadViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(DownloadViewModel), typeof(DownloadView), new PropertyMetadata(null));

    }
}
