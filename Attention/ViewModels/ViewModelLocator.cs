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
            SimpleIoc.Default.Register(() => new AppSettings(), nameof(AppSettings), false);
            SimpleIoc.Default.Register(() => new PixabayService(App.API_KEY), nameof(PixabayService), false);
            #endregion

            SimpleIoc.Default.Register<ShellViewModel>();
        }

        public AppSettings AppSettings => ServiceLocator.Current.GetInstance<AppSettings>(nameof(AppSettings));
        public PixabayService Pixabay => ServiceLocator.Current.GetInstance<PixabayService>(nameof(PixabayService));

        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public T GetService<T>() where T : class => ServiceLocator.Current.GetInstance<T>();
    }
}
