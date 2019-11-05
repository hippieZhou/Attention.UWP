﻿using Attention.UWP.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
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

            #region Services
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
            SimpleIoc.Default.Register(() => new DAL(App.Settings.DbPath));
            SimpleIoc.Default.Register(() => new PixabayService(App.API_KEY));
            #endregion

            #region ViewModels
            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<LocalViewModel>();
            SimpleIoc.Default.Register<MoreViewModel>();
            #endregion
        }

        public ILogManager LogManager => ServiceLocator.Current.GetInstance<ILogManager>();
        public DAL DAL => ServiceLocator.Current.GetInstance<DAL>();
        public PixabayService Pixabay => ServiceLocator.Current.GetInstance<PixabayService>();
        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SearchViewModel Search => ServiceLocator.Current.GetInstance<SearchViewModel>();
        public LocalViewModel Local => ServiceLocator.Current.GetInstance<LocalViewModel>();
        public MoreViewModel More => ServiceLocator.Current.GetInstance<MoreViewModel>();
    }
}
