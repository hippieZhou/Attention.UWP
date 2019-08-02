using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace ToolkitX.Extensions
{
    public static partial class CompositionExtensions
    {
        public static Visual Visual(this UIElement element) => ElementCompositionPreview.GetElementVisual(element);
    }
}
