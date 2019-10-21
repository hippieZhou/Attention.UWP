using Attention.UWP.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.UserControls
{
    public sealed partial class PhotoGridView : UserControl
    {
        public PhotoGridView()
        {
            this.InitializeComponent();
        }

        public PhotoGridViewModel ViewModel
        {
            get { return (PhotoGridViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PhotoGridViewModel), typeof(PhotoGridView), new PropertyMetadata(null));

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(PhotoGridView), new PropertyMetadata(null));

        private void PhotoGridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is AdaptiveGridView gridView && gridView.SelectedItem != null)
            {
                gridView.ScrollIntoView(gridView.SelectedItem);
            }
        }
    }
}
