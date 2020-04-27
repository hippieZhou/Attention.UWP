using Attention.App.Extensions;
using Attention.App.ViewModels.UcViewModels;
using System;
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
                    var scrollRoot = handler.FindVisualChild<ScrollViewer>();
                    if (scrollRoot != null)
                    {
                        scrollRoot.ViewChanged += async (sender, args) =>
                        {
                            if (viewmodel.Entities.IsLoading)
                                return;
                            if (scrollRoot.VerticalOffset <= scrollRoot.ScrollableHeight - 500) return;

                            Trace.WriteLine($"{scrollRoot.VerticalOffset}:{scrollRoot.ScrollableHeight}");

                            await viewmodel.Entities.LoadMoreItemsAsync(10);
                        };
                    }
                    #endregion

                    viewmodel.LoadCommand.Execute(null);
                }
            }));
    }
}
