using Attention.Core.Bus;
using Attention.Core.Commands;
using Attention.Core.Context;
using Attention.Core.Mappings;
using Attention.Core.Services;
using Attention.Core.Uow;
using AutoMapper;
using Microsoft.Practices.Unity;
using PixabaySharp;
using Unsplasharp;

namespace Attention.Core
{
    public static class DependencyInjection
    {
        public static IUnityContainer RegisterApplicationCore(this IUnityContainer container, string dbFile)
        {
            #region UOW
            container.RegisterInstance<IApplicationDbContext>(new ApplicationDbContext(dbFile));
            container.RegisterType<IDateTime, MachineDateTime>();
            container.RegisterType(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            #endregion

            #region WebAPI
            container.RegisterInstance(new PixabaySharpClient("12645414-59a5251905dfea7b916dd796f"));
            container.RegisterInstance(new UnsplasharpClient("xtU9WrbC5zUgMhkHAoNnq1La-vaVZYa8pxMtf-XiLgU", "gjKCMX5mopNYC7WBg8psV8iozNOTTRfUfWCeP-UADXY"));
            #endregion

            #region Mapping

            container.RegisterInstance<IMapper>(new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            })));
            #endregion

            #region CQRS

            container.RegisterType<PixabayCommand>();
            container.RegisterType<PixabayCommandHandler>();

            container.RegisterType<UnsplashCommand>();
            container.RegisterType<UnsplashCommandHandler>();

            container.RegisterType<DownloadCommand>();
            container.RegisterType<DownloadCommandHandler>();

            container.RegisterType<IMediatorHandler, InMemoryBus>();

            #endregion

            container.RegisterType<IDataService, DesignDataService>();

            return container;
        }
    }
}
