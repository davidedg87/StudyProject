using OrderManagement.Core.Cache;

namespace OrderManagement.API.Middlewares
{
    public class CachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CachingMiddleware> _logger;

        public CachingMiddleware(RequestDelegate next, IServiceProvider serviceProvider, ILogger<CachingMiddleware> logger)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            // Verifica se il metodo della richiesta è GET
            if (!string.Equals(context.Request.Method, HttpMethods.Get, StringComparison.OrdinalIgnoreCase))
            {
                await _next(context); // Passa al middleware successivo senza caching
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                //I middleware vengono registrati di default come transient. Non è possibile quindi iniettare all'interno servizi registrati con lifetime Scoped
                //Le soluzioni possibili sono di registrare il servizio ICacheService come transient oppure recuperarlo tramite serviceProveider dentro il middleware
                //stesso per evitare l'iniezione diretta
                var _cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                string cacheKey = context.Request.Path.ToString();
                string? cachedResponse = await _cacheService.GetCachedValueAsync(cacheKey);

                if (cachedResponse != null)
                {
                    _logger.LogInformation("Retrieved from cache : Key {CacheKey} , Value {CachedResponse}", cacheKey, cachedResponse);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(cachedResponse);
                    return;
                }

                using (var responseBody = new MemoryStream())
                {
                    var originalBodyStream = context.Response.Body;
                    context.Response.Body = responseBody;

                    await _next(context);

                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    await _cacheService.SetCachedValueAsync(cacheKey, responseText, TimeSpan.FromSeconds(15));

                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }

    }


}
