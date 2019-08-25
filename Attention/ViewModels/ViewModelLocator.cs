using System.IO;
using Attention.Models;
using Attention.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Attention.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        public readonly string ToastToken = "ToastNotification";

        private static ViewModelLocator _current;
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() =>
            {
                IConfiguration Configuration = OnStartup().Build();

                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection, Configuration);
                return serviceCollection.BuildServiceProvider();

                IConfigurationBuilder OnStartup()
                {
                    IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                    //todo:https://github.com/serilog/serilog-settings-configuration

                    return builder;
                }

                void ConfigureServices(IServiceCollection services, IConfiguration configuration)
                {
                    services.AddSingleton(configuration.GetSection(nameof(AppSettings)).Get<AppSettings>());

                    services.AddSingleton<PixabayService>();
                    services.AddSingleton<DownloadService>();
                    services.AddSingleton<AppSettingService>();
                    services.AddSingleton<LogService>();

                    services.AddSingleton<ExtendedSplashScreenViewModel>();
                }
            });

            SimpleIoc.Default.Register<ShellViewModel>();
        }

        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public T GetRequiredService<T>() where T : class => ServiceLocator.Current.GetInstance<ServiceProvider>().GetRequiredService<T>();
    }
}
