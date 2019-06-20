using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using HENG.Core.Services;
using HENG.Models;
using HENG.Services;
using HENG.UserControls;
using HENG.Views;

namespace HENG.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        public static Dictionary<string, Type> ControlsByKey { get; private set; } = new Dictionary<string, Type>();
       
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            Configure(typeof(HomeViewModel).FullName, typeof(HomeView));
            Configure(typeof(LocalViewModel).FullName, typeof(LocalControl));
            Configure(typeof(SettingsViewModel).FullName, typeof(SettingsControl));

            SimpleIoc.Default.Register(() => new DbContext());
            SimpleIoc.Default.Register(() => new PixabayService());
            SimpleIoc.Default.Register(() => new LoggingService());

            SimpleIoc.Default.Register<PhotoViewModel>();
            SimpleIoc.Default.Register<PhotoInfoViewModel>();

            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<LocalViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
        }

        private static void Configure(string key, Type controlType)
        {
            lock (ControlsByKey)
            {
                if (ControlsByKey.ContainsKey(key))
                {
                    throw new ArgumentException("This key is already used: " + key);
                }
                if (ControlsByKey.Any(p => p.Value.GetType() == controlType))
                {
                    throw new ArgumentException("This type is already configured with key " + ControlsByKey.First(p => p.Value.GetType() == controlType).Key);
                }
                ControlsByKey.Add(key, controlType);
            }
        }

        public DbContext Db => ServiceLocator.Current.GetInstance<DbContext>();
        public PixabayService Px => ServiceLocator.Current.GetInstance<PixabayService>();
        public LoggingService Log => ServiceLocator.Current.GetInstance<LoggingService>();

        public PhotoViewModel Photo => ServiceLocator.Current.GetInstance<PhotoViewModel>();
        public PhotoInfoViewModel PhotoInfo => ServiceLocator.Current.GetInstance<PhotoInfoViewModel>();

        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public LocalViewModel Local => ServiceLocator.Current.GetInstance<LocalViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }
}
