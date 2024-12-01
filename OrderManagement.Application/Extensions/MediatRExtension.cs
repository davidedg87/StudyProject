using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.PipelineBehaviours;
using System.Reflection;

namespace OrderManagement.Application.Extensions
{
    public static class MediatRExtension
    {
        public static void AddMediatRServices(this IServiceCollection services, IConfiguration configuration)
        {

           services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
           services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipelineBehavior<,>));
        }
    }
}
