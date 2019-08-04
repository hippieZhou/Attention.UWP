using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.UWP.Helpers
{
    public class TabNavItemSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item)
        {
            return base.SelectTemplateCore(item);
        }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return base.SelectTemplateCore(item, container);
        }
    }
}
