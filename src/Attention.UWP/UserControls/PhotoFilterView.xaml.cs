using Attention.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.UserControls
{
    public sealed partial class PhotoFilterView : UserControl
    {
        public PhotoFilterView()
        {
            this.InitializeComponent();
        }

        public PhotoFilterViewModel ViewModel
        {
            get { return (PhotoFilterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PhotoFilterViewModel), typeof(PhotoFilterView), new PropertyMetadata(null,(d,e)=> 
            {
                if (d is PhotoFilterView handler)
                {
                    handler.DataContext = e.NewValue;
                }
            }));
    }
}
