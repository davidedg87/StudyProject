using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManagement.Common.Extensions
{
    public static class CacheExtensions
    {
        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{configuration["Redis:Host"]}:6379"; // Indirizzo del tuo server Redis
                options.InstanceName = "SampleInstance"; // Nome dell'istanza (opzionale)
            });
        }
    }
}
