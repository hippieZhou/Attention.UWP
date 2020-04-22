using Attention.App.Framework;
using Microsoft.Practices.Unity;
using System;

namespace Attention.App
{
    public sealed class GeneralEngine : IEngine
    {
        private readonly Lazy<IUnityContainer> _container;
        public GeneralEngine(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            _container = new Lazy<IUnityContainer>(() => container);
        }

        public T Resolve<T>() where T : class => _container.Value.Resolve<T>();

        public T Resolve<T>(string name) where T : class => _container.Value.Resolve<T>(name);
    }
}
