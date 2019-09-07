using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Attention.UserControls
{
    public sealed partial class CommandControl : UserControl
    {
        public CommandControl()
        {
            this.InitializeComponent();
        }

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(CommandControl), new PropertyMetadata(null));

        public ICommand DownloadCommand
        {
            get { return (ICommand)GetValue(DownloadCommandProperty); }
            set { SetValue(DownloadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DownloadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DownloadCommandProperty =
            DependencyProperty.Register("DownloadCommand", typeof(ICommand), typeof(CommandControl), new PropertyMetadata(null));



        public ICommand MoreCommand
        {
            get { return (ICommand)GetValue(MoreCommandProperty); }
            set { SetValue(MoreCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoreCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoreCommandProperty =
            DependencyProperty.Register("MoreCommand", typeof(ICommand), typeof(CommandControl), new PropertyMetadata(null));
    }
}
