using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Extensions;

namespace HENG
{
    [TemplatePart(Name = PART_FrontPanel, Type = typeof(Grid))]
    [TemplatePart(Name = PART_BackPanel, Type = typeof(Grid))]
    [TemplatePart(Name = PATH_FrontContentPresenter, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PATH_BackContentPresenter, Type = typeof(ContentPresenter))]
    public sealed class FlipPanel : Control
    {
        private const string PART_FrontPanel = "PART_FrontPanel";
        private const string PART_BackPanel = "PART_BackPanel";
        private const string PATH_FrontContentPresenter = "PATH_FrontContentPresenter";
        private const string PATH_BackContentPresenter = "PATH_BackContentPresenter";
        private Grid _frontPanel;
        private Grid _backPanel;
        private Visual _frontPanelVisual;
        private Visual _backPanelVisual;

        public FlipPanel()
        {
            this.DefaultStyleKey = typeof(FlipPanel);
        }

        public object FrontContent
        {
            get { return (object)GetValue(FrontContentProperty); }
            set { SetValue(FrontContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FrontContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrontContentProperty =
            DependencyProperty.Register("FrontContent", typeof(object), typeof(FlipPanel), new PropertyMetadata(null));

        public object BackContent
        {
            get { return (object)GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }
        // Using a DependencyProperty as the backing store for BackContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackContentProperty =
            DependencyProperty.Register("BackContent", typeof(object), typeof(FlipPanel), new PropertyMetadata(null));

        public bool IsFlipped
        {
            get { return (bool)GetValue(IsFlippedProperty); }
            set { SetValue(IsFlippedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFlipped.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFlippedProperty =
            DependencyProperty.Register("IsFlipped", typeof(bool), typeof(FlipPanel), new PropertyMetadata(false));

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _frontPanel = GetTemplateChild<Grid>(PART_FrontPanel);
            _backPanel = GetTemplateChild<Grid>(PART_BackPanel);

            _frontPanelVisual = VisualExtensions.GetVisual(_frontPanel);
            _frontPanelVisual.CenterPoint = new Vector3((float)(_frontPanel.ActualWidth / 2), (float)(_frontPanel.ActualHeight / 2), 0f);
            _frontPanel.Opacity = 1;

            _backPanelVisual = VisualExtensions.GetVisual(_backPanel);
            _backPanelVisual.CenterPoint = new Vector3((float)(_backPanel.ActualWidth / 2), (float)(_backPanel.ActualHeight / 2), 0f);
            _backPanelVisual.RotationAngleInDegrees = 180f;
            _backPanel.Opacity = 0;


            SizeChanged += (sender, e) => 
            {
                var visual = VisualExtensions.GetVisual(this);
                UpdatePerspective(visual);
            };
            Tapped += (sender, e) =>
            {
                IsFlipped = !IsFlipped;
                //ScalarKeyFrameAnimation rotationAnimation = GoToState(IsFlipped);
            };
        }


        //private ScalarKeyFrameAnimation GoToState(bool isFlipped)
        //{
        //    LinearEasingFunction linear = _compositor.CreateLinearEasingFunction();
        //    ScalarKeyFrameAnimation rotationAnimation = _compositor.CreateScalarKeyFrameAnimation();
        //    if (isFlipped)
        //    {
        //        rotationAnimation.InsertKeyFrame(0, 0, linear);
        //        rotationAnimation.InsertKeyFrame(1, 250f, linear);
        //    }
        //    else
        //    {
        //        rotationAnimation.InsertKeyFrame(0, 250f, linear);
        //        rotationAnimation.InsertKeyFrame(1, 0f, linear);
        //    }

        //    rotationAnimation.Duration = TimeSpan.FromMilliseconds(800);

        //    return rotationAnimation;
        //}

        private void UpdatePerspective(Visual visual)
        {
            Vector2 sizeList = new Vector2((float)ActualWidth, (float)ActualHeight);

            Matrix4x4 perspective = new Matrix4x4(
                        1.0f, 0.0f, 0.0f, 0.0f,
                        0.0f, 1.0f, 0.0f, 0.0f,
                        0.0f, 0.0f, 1.0f, -1.0f / sizeList.X,
                        0.0f, 0.0f, 0.0f, 1.0f);

            visual.TransformMatrix =
                               Matrix4x4.CreateTranslation(-sizeList.X / 2, -sizeList.Y / 2, 0f) *      // Translate to origin
                               perspective *                                                            // Apply perspective at origin
                               Matrix4x4.CreateTranslation(sizeList.X / 2, sizeList.Y / 2, 0f);         // Translate back to original position
        }

        private T GetTemplateChild<T>(string name, string message = null) where T : DependencyObject
        {
            if (GetTemplateChild(name) is T child)
            {
                return child;
            }

            if (message == null)
            {
                message = $"{name} should not be null! Check the default Generic.xaml.";
            }

            throw new NullReferenceException(message);
        }
    }
}
