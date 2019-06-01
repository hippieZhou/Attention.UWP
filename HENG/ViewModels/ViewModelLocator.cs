using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using HENG.Core.Services;
using HENG.Views;

namespace HENG.ViewModels
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
            nav.Configure(typeof(HomeViewModel).FullName, typeof(HomeView));
            nav.Configure(typeof(SettingsViewModel).FullName, typeof(SettingsView));
            nav.Configure(typeof(DownloadViewModel).FullName, typeof(DownloadView));

            SimpleIoc.Default.Register(() => nav);

            SimpleIoc.Default.Register(() => new PixabayService("12645414-59a5251905dfea7b916dd796f"));

            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<DownloadViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
        }

        public PixabayService PxService => ServiceLocator.Current.GetInstance<PixabayService>();
        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public DownloadViewModel Download => ServiceLocator.Current.GetInstance<DownloadViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }
}
