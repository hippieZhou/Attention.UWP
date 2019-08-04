using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ToolkitX.Controls
{
    public sealed class TabItem : ContentControl
    {
        public TabItem()
        {
            DefaultStyleKey = typeof(TabItem);
        }

        #region Properties

        public IconElement Icon
        {
            get { return (IconElement)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IconElement), typeof(TabItem), new PropertyMetadata(null));

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(TabItem), new PropertyMetadata(null));

        #endregion Properties

        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}