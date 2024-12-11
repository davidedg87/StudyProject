using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Common.Authentication;
using OrderManagement.Infrastructure.Authentication;

namespace OrderManagement.Infrastructure.Extensions
{
    public static class ValidatorExtensions
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IApiKeyValidator, ApiKeyValidator>();
        }

    }
}
