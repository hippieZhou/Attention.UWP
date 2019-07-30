using HENG.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.UWP.UserControls
{
    public sealed partial class PhotoSearchView : UserControl
    {
        public PhotoSearchView()
        {
            this.InitializeComponent();
        }

        public PhotoSearchViewModel ViewModel
        {
            get { return (PhotoSearchViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PhotoSearchViewModel), typeof(PhotoSearchView), new PropertyMetadata(null));
    }
}
