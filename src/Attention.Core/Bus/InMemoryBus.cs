using Attention.Core.Commands;
using Attention.Core.Framework;
using Prism.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Core.Bus
{
    public class InMemoryBus : IMediatorHandler
    {
        private readonly ILoggerFacade _logger;

        public InMemoryBus(ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TRESPONSE> Send<TRESPONSE>(Command<TRESPONSE> request, CancellationToken cancellationToken = default)
        {
            var message = JsonSerializer.Serialize(request, request.GetType());
            _logger.Log(message, Category.Debug, Priority.None);

            var handlerName = $"{request.GetType().Name}Handler";
            var handlerType = Assembly.GetAssembly(request.GetType()).GetExportedTypes().FirstOrDefault(x => x.Name == handlerName);
            if (handlerType == null)
            {
                throw new ArgumentNullException($"类型未找到 {handlerName}");
            }

            var handler = EnginContext.Current.Resolve(handlerType);
            var methodInfo = handler.GetType().GetMethod("Handle");
            var result = methodInfo?.Invoke(handler, new object[] { request, cancellationToken }) as Task<TRESPONSE>;
            return await result;
        }
    }
}
