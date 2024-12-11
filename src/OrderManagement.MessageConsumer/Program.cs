using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderManagement.Application.Extensions;
using OrderManagement.Common.Extensions;
using OrderManagement.Messaging.Consumer.Consumers;
using Serilog;

namespace OrderManagement.Consumer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // Configura Serilog
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Sostituisci il token %BaseDirectory% con il percorso corrente
            configuration["Serilog:WriteTo:1:Args:path"] =
                configuration["Serilog:WriteTo:1:Args:path"]!.Replace("%BaseDirectory%", AppContext.BaseDirectory);

            var elasticUrl = configuration["Elastic:Url"];

            configuration["Serilog:WriteTo:2:Args:nodeUris"] =
                configuration["Serilog:WriteTo:2:Args:nodeUris"]!.Replace("%ElasticUrl%", elasticUrl);


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting up...");

                var host = Host.CreateDefaultBuilder(args)
                    .UseSerilog() // Aggiunge Serilog come provider di log
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddConfiguration(configuration); // Aggiunge la configurazione caricata
                    })
                    .ConfigureServices((context, services) =>
                    {

                        services.ConfigureServices();
                        services.AddCaching(configuration);
                        services.AddDbContexts(configuration);

                        // Configura MassTransit
                        services.AddMassTransit(x =>
                        {
                            x.AddConsumer<OrderConsumer>();
                            x.AddConsumer<OrderStatisticsConsumer>();

                            var rabbitMqHost = context.Configuration["RabbitMq:Host"];

                            x.UsingRabbitMq((context, cfg) =>
                            {
                                cfg.Host($"rabbitmq://{rabbitMqHost}");
                                cfg.ReceiveEndpoint("order-queue", e =>
                                {
                                    e.ConfigureConsumer<OrderConsumer>(context);
                                });
                                cfg.ReceiveEndpoint("orderStatistics-queue", e =>
                                {
                                    e.ConfigureConsumer<OrderStatisticsConsumer>(context);
                                });
                            });
                        });
                    })
                    .Build();

                await host.StartAsync();

                try
                {
                    Log.Information("Consumer is running. Press Ctrl+C to exit.");

                    // Mantiene il processo attivo finché non viene chiuso
                    var waitForShutdown = new TaskCompletionSource();
                    AppDomain.CurrentDomain.ProcessExit += (s, e) => waitForShutdown.TrySetResult();
                    Console.CancelKeyPress += (s, e) =>
                    {
                        e.Cancel = true;
                        waitForShutdown.TrySetResult();
                    };

                    await waitForShutdown.Task; // Attende che l'applicazione venga chiusa
                }
                finally
                {
                    await host.StopAsync();
                    Log.Information("Consumer stopped.");
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                await Log.CloseAndFlushAsync(); // Chiude e scarica i log rimanenti
            }
        }
    }
}
