using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.App.Behaviors
{
    public class ImageScrollBehavior : DependencyObject, IBehavior
    {
        private const int scrollViewerThresholdValue = 480;
        private ScrollViewer scrollViewer;
        private ListViewBase listGridView;

        public DependencyObject AssociatedObject { get; private set; }

        public Control TargetControl
        {
            get { return (Control)GetValue(TargetControlProperty); }
            set { SetValue(TargetControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetControlProperty =
            DependencyProperty.Register("TargetControl", typeof(Control), typeof(ImageScrollBehavior), new PropertyMetadata(null));

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            if (!GetScrollViewer())
            {
                ((ListViewBase)associatedObject).Loaded += ListGridView_Loaded;
            }
        }

        private void ListGridView_Loaded(object sender, RoutedEventArgs e)
        {
            GetScrollViewer();
            listGridView = sender as ListViewBase;
        }

        private bool GetScrollViewer()
        {
            scrollViewer = TargetControl as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.ViewChanging +=  (sender, e) =>
                {
                    //https://www.cnblogs.com/Damai-Pang/p/5209093.html
                    double verticalOffset = ((ScrollViewer)sender).VerticalOffset;
                    if (scrollViewer.ScrollableHeight - verticalOffset >= scrollViewerThresholdValue)
                    {
                        return;
                    }
                };
                return true;
            }
            return false;
        }

        public void Detach()
        {
            ((ListViewBase)AssociatedObject).Loaded -= ListGridView_Loaded;
            AssociatedObject = null;
        }
    }
}
