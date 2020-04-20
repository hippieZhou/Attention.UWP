using Prism.Logging;
using Serilog;
using System;

namespace Attention.App.Extensions
{
    public class SerilogLoggerFacade : ILoggerFacade
    {
        private readonly ILogger _loggerPriorityNone;
        private readonly ILogger _loggerPriorityHigh;
        private readonly ILogger _loggerPriorityMedium;
        private readonly ILogger _loggerPriorityLow;

        private readonly ILoggerFacade _next;

        public SerilogLoggerFacade(ILogger logger, ILoggerFacade next = null)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            var contextLogger = logger.ForContext<SerilogLoggerFacade>();
            _loggerPriorityNone = contextLogger.ForContext(nameof(Priority), nameof(Priority.None));
            _loggerPriorityHigh = contextLogger.ForContext(nameof(Priority), nameof(Priority.High));
            _loggerPriorityMedium = contextLogger.ForContext(nameof(Priority), nameof(Priority.Medium));
            _loggerPriorityLow = contextLogger.ForContext(nameof(Priority), nameof(Priority.Low));

            _next = next;
        }

        public void Log(string message, Category category, Priority priority)
        {
            ILogger logger;

            switch (priority)
            {
                case Priority.None:
                    logger = _loggerPriorityNone;
                    break;
                case Priority.High:
                    logger = _loggerPriorityHigh;
                    break;
                case Priority.Medium:
                    logger = _loggerPriorityMedium;
                    break;
                case Priority.Low:
                    logger = _loggerPriorityLow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(priority), priority, $"Unknown {nameof(Priority)}: `{priority}`");
            }

            switch (category)
            {
                case Category.Debug:
                    logger.Debug(message);
                    break;
                case Category.Info:
                    logger.Information(message);
                    break;
                case Category.Warn:
                    logger.Warning(message);
                    break;
                case Category.Exception:
                    logger.Error(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, $"Unknown {nameof(Category)}: `{category}`");
            }

            _next?.Log(message, category, priority);
        }
    }
}
