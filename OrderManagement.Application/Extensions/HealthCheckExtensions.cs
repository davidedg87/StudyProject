using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Infrastructure.Data.DbContext;

namespace OrderManagement.Application.Extensions
{
    public static class HealthCheckExtensions
    {
        public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
            .AddNpgSql(
                connectionString: configuration["ConnectionStrings:DefaultConnection"]!,
                name: "postgresql",
                tags: new[] { "db", "postgres" })
            .AddRabbitMQ(
                      $"amqp://{configuration["RabbitMq:User"]}:{configuration["RabbitMq:Password"]}@{configuration["RabbitMq:Host"]}:5672/",
                      name: "rabbitmq_health_check",
                      tags: new[] { "rabbitmq" })
            .AddElasticsearch(
                    configuration["Elastic:Url"]!,
                    name: "elasticsearch_health_check",
                    tags: new[] { "elasticsearch" })
            .AddRedis($"{configuration["Redis:Host"]}:6379", name: "Redis Health Check")
            .AddDbContextCheck<AppDbContext>();
        }
    }
}
