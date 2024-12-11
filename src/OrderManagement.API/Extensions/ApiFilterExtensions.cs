using OrderManagement.API.Filters;

namespace OrderManagement.API.Extensions
{
    public static class ApiFilterExtensions
    {
        public static void AddApiFilters(this IServiceCollection services)
        {
            services.AddScoped<ApiKeyAuthorizationFilter>();
        }

    }
}
