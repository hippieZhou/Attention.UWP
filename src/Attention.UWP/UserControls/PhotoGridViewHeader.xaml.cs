using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.UserControls
{
    public sealed partial class PhotoGridViewHeader : UserControl
    {
        public PhotoGridViewHeader()
        {
            this.InitializeComponent();
        }

        public Visibility RetryVisibility
        {
            get { return (Visibility)GetValue(RetryVisibilityProperty); }
            set { SetValue(RetryVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RetryVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RetryVisibilityProperty =
            DependencyProperty.Register("RetryVisibility", typeof(Visibility), typeof(PhotoGridViewHeader), new PropertyMetadata(Visibility.Visible));

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RefreshCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(PhotoGridViewHeader), new PropertyMetadata(null));
    }
}
