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
            //refreshBtn.Translation += new Vector3(0, 0, 32);
            //refreshBtn.Click += async (sender, e) =>
            //{
            //    var anim = refreshBtn.Rotate(value: 720.0f,
            //        centerX: refreshBtn.ActualSize.X / 2, centerY: refreshBtn.ActualSize.Y / 2,
            //        duration: 5000, delay: 400, easingType: EasingType.Back);
            //    anim.Then().Rotate(value: 0,
            //        centerX: refreshBtn.ActualSize.X / 2, centerY: refreshBtn.ActualSize.Y / 2,
            //        duration: 2500);
            //    await anim.StartAsync();
            //};
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
