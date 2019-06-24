using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
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

            SimpleIoc.Default.Register(() => new PixabayService());

            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<LocalViewModel>();
            SimpleIoc.Default.Register<MoreViewModel>();
        }

        public PixabayService Pix => ServiceLocator.Current.GetInstance<PixabayService>();

        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public LocalViewModel Local => ServiceLocator.Current.GetInstance<LocalViewModel>();
        public MoreViewModel More => ServiceLocator.Current.GetInstance<MoreViewModel>();
    }
}
