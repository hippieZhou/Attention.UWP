using HENG.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.Helpers
{
    public class ExDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate BingTemplate { get; set; }
        public DataTemplate PaperTemplate { get; set; }
        public DataTemplate PicsumTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var type = item?.GetType();

            if (type == typeof(BingItem))
            {
                return BingTemplate;
            }
            else if (type == typeof(PaperItem))
            {
                return PaperTemplate;
            }
            else if (type == typeof(PicsumItem))
            {
                return PicsumTemplate;
            }
            else
            {
                return base.SelectTemplateCore(item, container);
            }
        }
    }
}
