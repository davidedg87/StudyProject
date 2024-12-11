using OrderManagement.Application.Extensions;
using OrderManagement.BackgroundServices; // Assicurati di avere il namespace corretto
using OrderManagement.Common.Extensions;
using Serilog;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

#region Configuration

// Carica appsettings.json
var configuration = new ConfigurationBuilder()
   .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Sostituisci il token %BaseDirectory% con il percorso corrente
configuration["Serilog:WriteTo:1:Args:path"] =
    configuration["Serilog:WriteTo:1:Args:path"]!.Replace("%BaseDirectory%", AppContext.BaseDirectory);

var elasticUrl = configuration["Elastic:Url"];

configuration["Serilog:WriteTo:2:Args:nodeUris"] =
    configuration["Serilog:WriteTo:2:Args:nodeUris"]!.Replace("%ElasticUrl%", elasticUrl);


#endregion

builder.Services.AddHttpClient();
builder.Services.ConfigureServices();
builder.Services.AddCaching(configuration);
builder.Services.AddDbContexts(configuration);

#region Logs

// Configura Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

// Configura il logging per il builder
builder.Logging.ClearProviders(); // Rimuove i provider di logging predefiniti
builder.Logging.AddSerilog(); // Aggiunge Serilog come provider di loggin

#endregion

builder.Services.AddMassTransitServices(configuration);

builder.Services.AddHostedService<OrderStatisticsMonitoringService>();
builder.Services.AddHostedService<OrderStatisticsThresholdNotificationService>();
builder.Services.AddHostedService<OrderCreatingService>();

var host = builder.Build();

await host.RunAsync();
