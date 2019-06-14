using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using HENG.Core.Services;
using HENG.Models;
using HENG.Services;
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
            nav.Configure(typeof(LocalViewModel).FullName, typeof(LocalView));

            SimpleIoc.Default.Register(() => nav);

            SimpleIoc.Default.Register(() => new DbContext());
            SimpleIoc.Default.Register(() => new PixabayService());
            SimpleIoc.Default.Register(() => new LoggingService());

            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<LocalViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
        }

        public DbContext Db => ServiceLocator.Current.GetInstance<DbContext>();
        public PixabayService Px => ServiceLocator.Current.GetInstance<PixabayService>();
        public LoggingService Log => ServiceLocator.Current.GetInstance<LoggingService>();

        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public LocalViewModel Local => ServiceLocator.Current.GetInstance<LocalViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }
}
