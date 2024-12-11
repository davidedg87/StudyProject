using OrderManagement.Common.Extensions;

namespace OrderManagement.Api.Extensions
{
    public static class ApiCacheExtensions
    {
        public static void AddApiCaching(this IServiceCollection services, IConfiguration configuration)
        {
            //Abilitazione del caching:
            //AddOutputCache() registra i servizi necessari per la cache di output, permettendo di usare il middleware di caching di output per ottimizzare le prestazioni.
            services.AddOutputCache();
            services.AddCaching(configuration);
        }
    }
}
