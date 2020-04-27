using Attention.App.Extensions;
using Attention.App.ViewModels.UcViewModels;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.App.UserControls
{
    public sealed partial class WallpaperExploreView : UserControl
    {
        public WallpaperExploreView()
        {
            this.InitializeComponent();
          
        }

        public WallpaperExploreViewModel ConcreteDataContext
        {
            get { return (WallpaperExploreViewModel)GetValue(ConcreteDataContextProperty); }
            set { SetValue(ConcreteDataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConcreteDataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConcreteDataContextProperty =
            DependencyProperty.Register("ConcreteDataContext", typeof(WallpaperExploreViewModel), typeof(WallpaperExploreView), new PropertyMetadata(default, (d, e) => 
            {
                if (d is WallpaperExploreView handler && e.NewValue is WallpaperExploreViewModel viewmodel)
                {
                    handler.FindName("DeferredGrid");

                    #region 增量加载
                    //https://www.cnblogs.com/Damai-Pang/p/5209093.html
                    var viewer = handler.FindVisualChild<ScrollViewer>();
                    if (viewer != null)
                    {
                        viewer.ViewChanged += (sender, args) =>
                        {
                            Trace.WriteLine($"{viewer.VerticalOffset}:{viewer.ScrollableHeight}");
                        };
                    }
                    #endregion

                    viewmodel.LoadCommand.Execute(null);
                }
            }));
    }
}
