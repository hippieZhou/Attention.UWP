using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using HENG.Model;
using CommonServiceLocator;
using HENG.Views;

namespace HENG.ViewModel
{
    public class ViewModelLocator
    {
        public static bool UseDesignTimeData => false;

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var nav = new NavigationService();
            nav.Configure(typeof(HomeViewModel).FullName, typeof(PageHome));
            nav.Configure(typeof(ShiViewModel).FullName, typeof(PageShi));
            nav.Configure(typeof(CiViewModel).FullName, typeof(PageCi));
            nav.Configure(typeof(QuViewModel).FullName, typeof(PageQu));
            nav.Configure(typeof(SettingsViewModel).FullName, typeof(PageSettings));
            SimpleIoc.Default.Register<INavigationService>(() => nav);

            SimpleIoc.Default.Register<IDialogService, DialogService>();

            if (ViewModelBase.IsInDesignModeStatic || UseDesignTimeData)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<ShellViewModel>();
        }

        public ShellViewModel Shll => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public ShiViewModel Shi => ServiceLocator.Current.GetInstance<ShiViewModel>();
        public CiViewModel Ci => ServiceLocator.Current.GetInstance<CiViewModel>();
        public QuViewModel Qu => ServiceLocator.Current.GetInstance<QuViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }
}
