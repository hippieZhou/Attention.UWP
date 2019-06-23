using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using HENG.App.Views;
using HENG.Core.Services;

namespace HENG.App.ViewModels
{

    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            var nav = new NavigationService();
            nav.Configure(typeof(HomeViewModel).FullName, typeof(HomePage));
            SimpleIoc.Default.Register(() => nav);

            SimpleIoc.Default.Register(() => new PixabayService());

            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
        }

        public PixabayService Pix => ServiceLocator.Current.GetInstance<PixabayService>();

        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
    }
}
