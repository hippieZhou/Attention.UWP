using Attention.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.UserControls
{
    public sealed partial class PhotoGridHeader : UserControl
    {
        public PhotoGridHeader()
        {
            this.InitializeComponent();
        }

        public PhotoGridHeaderViewModel ViewModel
        {
            get { return (PhotoGridHeaderViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PhotoGridHeaderViewModel), typeof(PhotoGridHeader), new PropertyMetadata(null));
    }
}
