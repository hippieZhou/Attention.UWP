using Windows.UI.Xaml.Navigation;
using HENG.ViewModels;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace HENG.Views
{
    public sealed partial class ShellPage
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;

        public ShellPage()
        {
            InitializeComponent();
            ViewModel.Initialize(ContentFrame, NavView, DetailView, NotifGrid);

            //DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            //{
            //    await this.Scale(0.5f, 0.5f, (float)this.ActualWidth / 2, (float)this.ActualHeight / 2, 200, 0, EasingType.Linear).StartAsync();
            //    var bounds = Window.Current.Bounds;
            //    double width = bounds.Width;
            //    double height = bounds.Height;
            //    float scaleX = (float)(width / 16);
            //    float scaleY = (float)(height / 9);
            //    var anim = this.Scale(scaleX, scaleY, (float)this.ActualWidth / 2, (float)this.ActualHeight / 2, 300, 0, EasingType.Linear);
            //    await anim.StartAsync();
            //});
        }


        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Cleanup();
            base.OnNavigatingFrom(e);
        }
    }
}
