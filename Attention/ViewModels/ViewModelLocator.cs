using Attention.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
namespace Attention.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {

        private static ViewModelLocator _current;
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            #region ConfigureServices
            SimpleIoc.Default.Register(() => new AppSettingService(), nameof(AppSettingService), false);
            SimpleIoc.Default.Register<PixabayService>();
            #endregion

            SimpleIoc.Default.Register<ExtendedSplashScreenViewModel>();
            SimpleIoc.Default.Register<ShellViewModel>();
        }

        public ExtendedSplashScreenViewModel ExtendedSplashScreen => ServiceLocator.Current.GetInstance<ExtendedSplashScreenViewModel>();
        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public T GetService<T>() where T : class => ServiceLocator.Current.GetInstance<T>();
    }
}
