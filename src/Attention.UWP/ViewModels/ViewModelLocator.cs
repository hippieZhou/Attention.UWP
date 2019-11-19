using Attention.UWP.Models;
using Attention.UWP.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MetroLog;
using MetroLog.Targets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Attention.UWP.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator() => ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

        public ILogManager LogManager => ServiceLocator.Current.GetInstance<ILogManager>();
        public DAL DAL => ServiceLocator.Current.GetInstance<DAL>();
        public PixabayService Pixabay => ServiceLocator.Current.GetInstance<PixabayService>();
        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SearchViewModel Search => ServiceLocator.Current.GetInstance<SearchViewModel>();
        public LocalViewModel Local => ServiceLocator.Current.GetInstance<LocalViewModel>();
        public MoreViewModel More => ServiceLocator.Current.GetInstance<MoreViewModel>();

        public async Task InitializeAsync()
        {
            var key = await LoadSecretAsync();

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
            SimpleIoc.Default.Register(() => new DAL(App.Settings.DbFile));
            SimpleIoc.Default.Register(() => new PixabayService(key));
            #endregion

            #region ViewModels
            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<LocalViewModel>();
            SimpleIoc.Default.Register<MoreViewModel>();
            #endregion
        }

        private async Task<string> LoadSecretAsync()
        {
            try
            {
                var file = Path.Combine(Package.Current.InstalledLocation.Path, "secret.json");
                StorageFile secret = await StorageFile.GetFileFromPathAsync(file);
                string json = await FileIO.ReadTextAsync(secret);
                var key = JsonConvert.DeserializeObject<JObject>(json)[nameof(API_KEY)].ToObject<API_KEY>();
#if DEBUG
                return key.Debug;
#else
                return key.Release;
#endif
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
