using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using HENG.Model;
using CommonServiceLocator;

namespace HENG.ViewModel
{
    public class ViewModelLocator
    {
        public static bool UseDesignTimeData => false;

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var nav = new NavigationService();
            nav.Configure(typeof(ShellViewModel).FullName, typeof(Shell));
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
    }
}
