using HENG.ViewModels;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace HENG.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;
        public ShellPage()
        {
            this.InitializeComponent();
            ViewModel.Initialize();
        }

        /// <summary>
        /// http://meanme.com/2017/07/11/sticky-header/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CompositionPropertySet scrollerPropertySet = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(MainScrollViewer);
            Compositor compositor = scrollerPropertySet.Compositor;

            // Get the visual that represents our HeaderTextBlock 
            // And define the progress animation string
            var headerVisual = ElementCompositionPreview.GetElementVisual(ScrollHeader);
            string progress = "Clamp(Abs(scroller.Translation.Y) / 100.0, 0.0, 1.0)";

            // Create the expression and add in our progress string.
            var textExpression = compositor.CreateExpressionAnimation("Lerp(1.5, 1, " + progress + ")");
            textExpression.SetReferenceParameter("scroller", scrollerPropertySet);

            // Shift the header by 50 pixels when scrolling down
            var offsetExpression = compositor.CreateExpressionAnimation($"-scroller.Translation.Y - {progress} * 50");
            offsetExpression.SetReferenceParameter("scroller", scrollerPropertySet);
            headerVisual.StartAnimation("Offset.Y", offsetExpression);
        }
    }
}
