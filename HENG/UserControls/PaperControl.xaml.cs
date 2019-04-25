using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.UserControls
{
    public sealed partial class PaperControl : UserControl
    {
        public PaperControl()
        {
            this.InitializeComponent();
        }

        public object Photos
        {
            get { return (object)GetValue(PhotosProperty); }
            set { SetValue(PhotosProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Photos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PhotosProperty =
            DependencyProperty.Register("Photos", typeof(object), typeof(PaperControl), new PropertyMetadata(null));

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RefreshCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(PaperControl), new PropertyMetadata(null));
    }
}
