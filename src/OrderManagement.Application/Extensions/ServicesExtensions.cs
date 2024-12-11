using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Services;
using OrderManagement.Core;
using OrderManagement.Core.Interfaces;
using OrderManagement.Infrastructure;
using OrderManagement.Infrastructure.Decorators;
using Scrutor;

namespace OrderManagement.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {


            /*
                FromAssemblies - allows you to specify which assemblies to scan
                AddClasses - adds the classes from the selected assemblies
                UsingRegistrationStrategy - defines which RegistrationStrategy to use
                AsMatchingInterface - registers the types as matching interfaces (ClassName → IClassName)
                WithScopedLifetime - registers the types with a scoped service lifetime
                There are three values for RegistrationStrategy you can use:

                RegistrationStrategy.Skip - skips registrations if service already exists
                RegistrationStrategy.Append- appends a new registration for existing services
                RegistrationStrategy.Throw- throws when trying to register an existing service
             
             */

            //SCAN ASSEMBLY E REGISTRAZIONE AUTOMATICA DEI SERVIZI CON SCRUTOR
            services.Scan(selector =>
             selector
             .FromAssemblies(typeof(MarkerInfrastructure).Assembly, typeof(MarkerCore).Assembly)
             .AddClasses(publicOnly: false)
             .UsingRegistrationStrategy(RegistrationStrategy.Skip)
             .AsMatchingInterface()
             .WithScopedLifetime()
             );

            //Esempio utilizzo decoratori in scrutor
            //Crea la seguente catena di decoratori: LoggingDecorator -> CachingDecorator -> OrderRepository
            //Questo significa che ogni qualvolta viene iniettato IOrderRepository viene restituita un istanza di CachingDecorator e quindi ogni metodo richiamato dell'interfaccia IOrderRepository passerà prima per gli strati di LoggingDecorator e CachingDecorator
       
            services.Decorate<IOrderRepository, CachingOrderRepositoryDecorator>();
            services.Decorate<IOrderRepository, LoggingOrderRepositoryDecorator>();

            //Vecchia modalità di salvare più servizi sulla stessa interfaccia. In questo caso solo l'ultimo viene restituito quando si chiede un INotificationService
            services.AddScoped<INotificationService, SmsNotificationService>();
            services.AddScoped<INotificationService, EmailNotificationService>();
            services.AddScoped<INotificationService, PushNotificationService>();

            //KEYED SERVICE
            services.AddKeyedScoped<INotificationService, SmsNotificationService>("SMS");
            services.AddKeyedScoped<INotificationService, EmailNotificationService>("EMAIL");
            services.AddKeyedScoped<INotificationService, PushNotificationService>("PUSH");

            services.AddScoped<NotifierService>();

            //services.AddKeyedScoped<ITestHttpService, TestHttpService>("IMPLICIT_FACTORY");
            services.AddKeyedScoped<ITestHttpService, TestHttpService2>("EXPLICIT_FACTORY_SCRATCH");
            services.AddKeyedScoped<ITestHttpService, TestHttpService3>("EXPLICIT_FACTORY_CONFIG");

            


        }

    }
}
