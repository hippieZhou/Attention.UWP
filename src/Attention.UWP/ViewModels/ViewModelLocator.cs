using Attention.UWP.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MetroLog;
using MetroLog.Targets;

namespace Attention.UWP.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register(() =>
            {
#if DEBUG
                LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget());
#else
                LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Error, LogLevel.Fatal, new StreamingFileTarget());
#endif
                GlobalCrashHandler.Configure();

                return LogManagerFactory.DefaultLogManager;
            });

            SimpleIoc.Default.Register(() => new PixabayService("3153915-c1b347f3736d73ef2cd6a0e79"), false);
            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<LocalViewModel>();
            SimpleIoc.Default.Register<MoreViewModel>();
        }

        public ILogManager LogManager => ServiceLocator.Current.GetInstance<ILogManager>();
        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SearchViewModel Search => ServiceLocator.Current.GetInstance<SearchViewModel>();
        public LocalViewModel Local => ServiceLocator.Current.GetInstance<LocalViewModel>();
        public MoreViewModel More => ServiceLocator.Current.GetInstance<MoreViewModel>();
    }
}
