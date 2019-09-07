using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.App.UserControls
{
    public sealed partial class TitleBarControl : UserControl
    {
        public TitleBarControl()
        {
            this.InitializeComponent();
        }

        public ICommand NavToDownloadCommand
        {
            get { return (ICommand)GetValue(NavToDownloadCommandProperty); }
            set { SetValue(NavToDownloadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavToDownloadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavToDownloadCommandProperty =
            DependencyProperty.Register("NavToDownloadCommand", typeof(ICommand), typeof(TitleBarControl), new PropertyMetadata(null));

        public ICommand NavToMoreCommand
        {
            get { return (ICommand)GetValue(NavToMoreCommandProperty); }
            set { SetValue(NavToMoreCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for NavToMoreCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavToMoreCommandProperty =
            DependencyProperty.Register("NavToMoreCommand", typeof(ICommand), typeof(TitleBarControl), new PropertyMetadata(null));
    }
}
