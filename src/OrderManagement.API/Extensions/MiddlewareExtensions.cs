using OrderManagement.API.Middlewares;

namespace OrderManagement.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder)
        {
            // Middleware per reindirizzare le richieste HTTP a HTTPS
            builder.UseHttpsRedirection();

            // Middleware di routing deve essere chiamato prima di altri middleware che dipendono da routing
            builder.UseRouting();

            // Aggiungi i middleware personalizzati
            builder.UseMiddleware<ApiKeyMiddleware>();
            builder.UseMiddleware<CorrelationIdMiddleware>(); 

            builder.UseMiddleware<ExceptionHandlingMiddleware>();
            //builder.UseExceptionHandler(); Può essere usato come alternativa a ExceptionHandlingMiddleware in quanto gestisce la stessa cosa
            builder.UseExceptionHandler();
            // Al momento commentato in quanto sembra che chiuda lo stream della richiesta http causando errori
            //builder.UseMiddleware<CachingMiddleware>();

            /*UseOutputCache() inserisce il middleware di caching di output nella pipeline di ASP.NET Core, permettendo al framework di:
            Intercettare le richieste: Quando arriva una richiesta per un endpoint con la cache di output abilitata, il middleware verifica se la risposta è già memorizzata in cache.
            Restituire risposte cached: Se la risposta è già in cache e valida, viene restituita immediatamente senza eseguire nuovamente la logica dell’endpoint.
            Gestire l’aggiornamento della cache: Se la risposta non è in cache o è scaduta, il middleware passa la richiesta al controller, memorizza il nuovo output e lo invia al client.
             */
            builder.UseOutputCache(); // Aggiunta del middleware di output cache

            builder.UseAuthorization(); // Aggiunta del middleware di autorizzazione
            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return builder;
        }
    }

  
}
