using Attention.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.UserControls
{
    public sealed partial class PhotoItemView : UserControl
    {
        public PhotoItemView()
        {
            this.InitializeComponent();
        }

        public PhotoItemViewModel ViewModel
        {
            get { return (PhotoItemViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PhotoItemViewModel), typeof(PhotoItemView), new PropertyMetadata(null));
    }
}
