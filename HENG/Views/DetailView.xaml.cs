using HENG.ViewModels;
using System;
using System.Numerics;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace HENG.Views
{
    public sealed partial class DetailView : Page
    {
        public DetailViewModel ViewModel => ViewModelLocator.Current.Detail;
        public DetailView()
        {
            this.InitializeComponent();
        }

        public object Photo
        {
            get { return (object)GetValue(PhotoProperty); }
            set { SetValue(PhotoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Photo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PhotoProperty =
            DependencyProperty.Register("Photo", typeof(object), typeof(DetailView), new PropertyMetadata(null,(d,e)=> 
            {
                if (d is DetailView handler)
                {
                    handler.ViewModel.Model = e.NewValue;
                    handler.ViewModel.RefreshCommand.Execute(null);
                }
            }));

        public ICommand BackCommand
        {
            get { return (ICommand)GetValue(BackCommandProperty); }
            set { SetValue(BackCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackCommandProperty =
            DependencyProperty.Register("BackCommand", typeof(ICommand), typeof(DetailView), new PropertyMetadata(null));

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var root = (UIElement)sender;
            InitializeAnimation(root);
        }

        private void InitializeAnimation(UIElement root)
        {
            var rootVisual = ElementCompositionPreview.GetElementVisual(root);
            var compositor = rootVisual.Compositor;

            var pointerEnteredAnimation = compositor.CreateVector3KeyFrameAnimation();
            pointerEnteredAnimation.InsertKeyFrame(1.0f, new Vector3(1.1f));

            var pointerExitedAnimation = compositor.CreateVector3KeyFrameAnimation();
            pointerExitedAnimation.InsertKeyFrame(1.0f, new Vector3(1.0f));


            root.PointerEntered += (sender, args) =>
            {
                rootVisual.CenterPoint = new Vector3(rootVisual.Size / 2, 0);
                rootVisual.StartAnimation("Scale", pointerEnteredAnimation);
            };
            root.PointerExited += (sender, args) => rootVisual.StartAnimation("Scale", pointerExitedAnimation);
        }
    }
}
