using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace OrderManagement.Common.Extensions
{
    public static class LoggingExtensions
    {
        public static void AddLogging(this IHostBuilder hostBuilder, IConfiguration configurationSettings)
        {
            hostBuilder.UseSerilog((context, services, configuration) =>
            {
                configuration.ReadFrom.Configuration(configurationSettings);
            });

        }
    }
}
