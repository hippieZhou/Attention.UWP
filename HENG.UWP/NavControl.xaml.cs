using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Linq;
using muxc = Microsoft.UI.Xaml.Controls;


namespace HENG.UWP
{
    public sealed partial class NavControl : UserControl
    {
        public NavControl()
        {
            this.InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < NavItems.Count; i++)
            {
                navView.MenuItems.Add(new muxc.NavigationViewItem
                {
                    Icon = NavItems[i].Icon,
                    Content = NavItems[i].Header
                });
                container.Items.Add(NavItems[i].Content);
            }
        }

        public IList<NavItem> NavItems
        {
            get { return (IList<NavItem>)GetValue(NavItemsProperty); }
            set { SetValue(NavItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavItemsProperty =
            DependencyProperty.Register("NavItems", typeof(IList<NavItem>), typeof(NavControl), new PropertyMetadata(new List<NavItem>()));

        public NavItem SelectedItem
        {
            get { return (NavItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(NavItem), typeof(NavControl), new PropertyMetadata(null, (d, e) => 
            {
                if (d is NavControl self && e.NewValue is NavItem item)
                {
                    self.navView.SelectedItem = self.navView.MenuItems
                    .OfType<muxc.NavigationViewItem>()
                    .FirstOrDefault(p => p.Icon == item.Icon && p.Content == item.Header);
                }
            }));

        public ICommand ItemInvokedCommand
        {
            get { return (ICommand)GetValue(ItemInvokedCommandProperty); }
            set { SetValue(ItemInvokedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemInvokedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemInvokedCommandProperty =
            DependencyProperty.Register("ItemInvokedCommand", typeof(ICommand), typeof(NavControl), new PropertyMetadata(null));

        public UIElement PaneFooter
        {
            get { return (UIElement)GetValue(PaneFooterProperty); }
            set { SetValue(PaneFooterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PaneFooter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PaneFooterProperty =
            DependencyProperty.Register("PaneFooter", typeof(UIElement), typeof(NavControl), new PropertyMetadata(null));

        private void NavView_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            SelectedItem = NavItems.FirstOrDefault(p => p.Header == args.InvokedItem);
            ItemInvokedCommand?.Execute(SelectedItem);
        }
    }

    public class NavItem: ContentControl
    {
        public IconElement Icon
        {
            get { return (IconElement)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IconElement), typeof(NavItem), new PropertyMetadata(null));

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(NavItem), new PropertyMetadata(null));

        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
