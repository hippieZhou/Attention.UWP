using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using HENG.UWP.Services;
using HENG.UWP.Views;

namespace HENG.UWP.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register(() => new NavigationServiceEx());

            SimpleIoc.Default.Register(() => new PixabayService(App.Get_API_KEY()));
            SimpleIoc.Default.Register(() => new DownloadService());

            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<DownloadViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();


            Register<HomeViewModel, HomePage>();
            Register<DownloadViewModel, DownloadPage>();
        }

        public PixabayService Pix => ServiceLocator.Current.GetInstance<PixabayService>();

        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public DownloadViewModel Download => ServiceLocator.Current.GetInstance<DownloadViewModel>();
        public SettingViewModel Setting => ServiceLocator.Current.GetInstance<SettingViewModel>();

        public NavigationServiceEx NavigationService => SimpleIoc.Default.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>() where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
