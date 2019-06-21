using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace HENG.UserControls
{
    public sealed partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            this.InitializeComponent();
        }

        public ScrollHeaderMode HeaderMode
        {
            get { return (ScrollHeaderMode)GetValue(HeaderModeProperty); }
            set { SetValue(HeaderModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderModeProperty =
            DependencyProperty.Register("HeaderMode", typeof(ScrollHeaderMode), typeof(HeaderControl), new PropertyMetadata(ScrollHeaderMode.None));

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for RefreshCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(HeaderControl), new PropertyMetadata(null));

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(HeaderControl), new PropertyMetadata(null));

        public ICommand ItemInvokedCommand
        {
            get { return (ICommand)GetValue(ItemInvokedCommandProperty); }
            set { SetValue(ItemInvokedCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ItemInvokedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemInvokedCommandProperty =
            DependencyProperty.Register("ItemInvokedCommand", typeof(ICommand), typeof(HeaderControl), new PropertyMetadata(null));

        private void AutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(DateTime.UtcNow);
        }
    }
}
