using Attention.App.ViewModels.UcViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Attention.App.UserControls
{
    public sealed partial class WallpaperCardView : UserControl
    {
        public WallpaperCardView()
        {
            this.InitializeComponent();
        }

        public WallpaperCardViewModel ConcreteDataContext
        {
            get { return (WallpaperCardViewModel)GetValue(ConcreteDataContextProperty); }
            set { SetValue(ConcreteDataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConcreteDataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConcreteDataContextProperty =
            DependencyProperty.Register("ConcreteDataContext", typeof(WallpaperCardViewModel), typeof(WallpaperCardView), new PropertyMetadata(null, (d, e) =>
             {
                 if (d is WallpaperCardView handler && e.NewValue is WallpaperCardViewModel vm)
                 {
                     vm.Initialize(handler.HeroImage, handler.Header);
                 }
             }));

        private void OnContentViewModeChanged(WinUI.TwoPaneView sender, object args)
        {

        }
    }
}
