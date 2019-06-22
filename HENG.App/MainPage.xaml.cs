using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HENG.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CompositionPropertySet _scrollerPropertySet;
        private Compositor _compositor;
        private CompositionPropertySet _props;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            header.Blur(30, 0, 0).Start();
            // Get the PropertySet that contains the scroll values from MyScrollViewer
            _scrollerPropertySet = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(MyScrollviewer);
            _compositor = _scrollerPropertySet.Compositor;

            // Create a PropertySet that has values to be referenced in the ExpressionAnimations below
            _props = _compositor.CreatePropertySet();
            _props.InsertScalar("progress", 0);
            _props.InsertScalar("clampSize", 100);

            // Get references to our property sets for use with ExpressionNodes
            var scrollingProperties = _scrollerPropertySet.GetSpecializedReference<ManipulationPropertySetReferenceNode>();
            var props = _props.GetReference();
            var progressNode = props.GetScalarProperty("progress");
            var clampSizeNode = props.GetScalarProperty("clampSize");

            // Create and start an ExpressionAnimation to track scroll progress over the desired distance
            var progressAnimation = ExpressionFunctions.Clamp(-scrollingProperties.Translation.Y / clampSizeNode, 0, 1);
            _props.StartAnimation("progress", progressAnimation);

            // Get the backing visual for the header so that its properties can be animated
            var headerVisual = ElementCompositionPreview.GetElementVisual(Header);

            // Create and start an ExpressionAnimation to clamp the header's offset to keep it onscreen
            var headerTranslationAnimation = ExpressionFunctions.Conditional(progressNode < 1, 0, -scrollingProperties.Translation.Y - clampSizeNode);
            headerVisual.StartAnimation("Offset.Y", headerTranslationAnimation);

            // Create and start an ExpressionAnimation to scale the header during overpan
            var headerScaleAnimation = ExpressionFunctions.Lerp(1, 1.25f, ExpressionFunctions.Clamp(scrollingProperties.Translation.Y / 50, 0, 1));
            headerVisual.StartAnimation("Scale.X", headerScaleAnimation);
            headerVisual.StartAnimation("Scale.Y", headerScaleAnimation);

            //Set the header's CenterPoint to ensure the overpan scale looks as desired
            headerVisual.CenterPoint = new Vector3((float)(Header.ActualWidth / 2), (float)Header.ActualHeight, 0);
        }
    }
}
