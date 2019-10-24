using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.UWP.UserControls
{
    public sealed partial class ShellNavBar : UserControl
    {
        public ShellNavBar()
        {
            this.InitializeComponent();
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ShellNavBar), new PropertyMetadata(null));

        public ICommand NavToDownloadCommand
        {
            get { return (ICommand)GetValue(NavToDownloadCommandProperty); }
            set { SetValue(NavToDownloadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavToDownloadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavToDownloadCommandProperty =
            DependencyProperty.Register("NavToDownloadCommand", typeof(ICommand), typeof(ShellNavBar), new PropertyMetadata(null));

        public ICommand NavToAboutCommand
        {
            get { return (ICommand)GetValue(NavToAboutCommandProperty); }
            set { SetValue(NavToAboutCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavToAboutCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavToAboutCommandProperty =
            DependencyProperty.Register("NavToAboutCommand", typeof(ICommand), typeof(ShellNavBar), new PropertyMetadata(null));

    }
}
