using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OrderManagement.Common.Factory;
using Polly;
using System.Data;

namespace OrderManagement.Application.Extensions
{
    public static class DbConnectionExtensions
    {
        public static void AddDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
           
            services.AddScoped<OpenNpgsqlConnectionFactory>();

            services.AddScoped<IDbConnection>(sp =>
            {
                var factory = sp.GetRequiredService<OpenNpgsqlConnectionFactory>();
                return factory.CreateOpenConnection();

            });
            
        }
    }
}
