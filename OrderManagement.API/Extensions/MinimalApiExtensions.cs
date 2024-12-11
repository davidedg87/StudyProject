using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.Extensions.Caching.Distributed;

namespace OrderManagement.API.Extensions
{
    public static class MinimalApiExtensions
    {
        public static void AddRedisAPI(this WebApplication app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .HasApiVersion(new ApiVersion(2))
                .ReportApiVersions()
                .Build();


            // Aggiungi un gruppo per i tag (equivalente al controller)
            RouteGroupBuilder redisGroup = app.MapGroup("/api/v{apiVersion:apiVersion}/redis")
                .WithApiVersionSet(apiVersionSet)
                .WithTags("Redis Cache"); // Associa un tag per raggruppare gli endpoint


            redisGroup.MapGet("set", async (IDistributedCache distributedCache, string key, string value) =>
            {
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1)); // Imposta il tempo di scadenza della cache

                // Imposta un valore nella cache
                await distributedCache.SetStringAsync(key, value, options);
                return Results.Ok($"Valore impostato per la chiave '{key}': '{value}'");
            }).MapToApiVersion(1);


            redisGroup.MapGet("get", async (IDistributedCache distributedCache, string key) =>
            {
                // Recupera un valore dalla cache
                var value = await distributedCache.GetStringAsync(key);
                if (value == null)
                {
                    return Results.NotFound($"Chiave '{key}' non trovata nella cache.");
                }
                return Results.Ok($"Valore recuperato per la chiave '{key}': '{value}'");
            }).MapToApiVersion(1);

            redisGroup.MapGet("remove", async (IDistributedCache distributedCache, string key) =>
            {
                // Rimuovi un valore dalla cache
                await distributedCache.RemoveAsync(key);
                return Results.Ok($"Chiave '{key}' rimossa dalla cache.");
            }).MapToApiVersion(1);

        }




        public static void AddExaminationAPI(this WebApplication app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
               .HasApiVersion(new ApiVersion(1))
               .HasApiVersion(new ApiVersion(2))
               .ReportApiVersions()
               .Build();

            // Aggiungi un gruppo per i tag (equivalente al controller)
            RouteGroupBuilder examinationGroup = app.MapGroup("/api/v{apiVersion:apiVersion}/examination")
                .WithApiVersionSet(apiVersionSet)
                .WithTags("Examination Minimal"); // Associa un tag per raggruppare gli endpoint


            examinationGroup.MapGet("api/examination/products", async () =>
            {
                return Results.Ok("Products");
            }).MapToApiVersion(1);


            examinationGroup.MapGet("api/examination/products/{id}", async (int id) =>

            {
                return Results.Ok("map get product id");

            }

            ).MapToApiVersion(1);


            examinationGroup.MapPost("api/examination/products", async (string products) =>
            {

                return Results.Ok("Product added");
            }).MapToApiVersion(1);


            examinationGroup.MapPut("api/examination/products/{id}", async (int id, string product) =>
            {

                return Results.Ok("Product updated");
            }).MapToApiVersion(1);

            examinationGroup.MapDelete("api/examination/products/{id}", async (int id) =>
            {
                return Results.Ok("product deleted");
            }).MapToApiVersion(1);



        }



















    






















    }
}
