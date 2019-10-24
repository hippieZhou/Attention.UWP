using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace HENG.UWP.Extensions
{
    public static class CompositionExtensions
    {
        public static Visual Visual(this UIElement element) => ElementCompositionPreview.GetElementVisual(element);
    }
}
