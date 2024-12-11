using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OrderManagement.Api.Extensions;
using OrderManagement.API.Extensions;
using OrderManagement.API.Handlers;
using OrderManagement.Application.Extensions;
using OrderManagement.Common.Extensions;
using OrderManagement.Infrastructure.Extensions;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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

#region Services 

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddValidators();
builder.Services.AddApiVersioningServices();
builder.Services.AddApiFilters();
builder.Services.AddApiCaching(configuration);
builder.Services.AddHealthChecks(configuration);
builder.Services.AddDbContexts(configuration);
builder.Services.AddDbContexts(configuration);
builder.Services.AddDbConnection(configuration);
builder.Host.AddLogging(configuration);
builder.Services.AddMassTransitServices(configuration);
builder.Services.AddMediatRServices(configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureServices();
//Necessario per gestire le eccezione con l'handler piuttosto che con il middleware
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpExtensions(configuration);
#endregion

#region Build

var app = builder.Build();

#endregion

#region Migrations

app.ApplyMigrations();

#endregion

#region Serilog

app.UseSerilogRequestLogging();

#endregion

#region Endpoint for Redis

//DEVE ESSERE CHIAMATO PRIMA DI DELLA GESTIONE DELLE API VERSION PER SWAGGER PER VEDERE SOLO LE VERSIONI ABILITATE
app.AddRedisAPI();
app.AddExaminationAPI();

#endregion

#region Swagger 

app.AddSwaggerServices();

#endregion

#region Middlewares

// Configura i middleware utilizzando il metodo di estensione
app.UseMiddlewares();

#endregion

#region Endpoint for HealthChecks

app.MapHealthChecks("health", new HealthCheckOptions {ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

#endregion

await app.RunAsync();
