using Microsoft.Extensions.Logging;

namespace OrderManagement.Common.Extensions
{
    public static class ILoggerExtensions
    {

        public static IDisposable? BeginCorrelationScope(this ILogger logger, string? correlationId = null)
        {
            correlationId ??= Guid.NewGuid().ToString();

            return logger.BeginScope(new Dictionary<string, string>
            {
                { "CorrelationId", correlationId }
            });


        }
    }
}
