using OrderManagement.API.Filters;

namespace OrderManagement.API.Middlewares
{
    public class ApiKeyMiddleware
    {
        //Utile per impostare una validazione globale per tutte le richieste senza andare ad esplicitarlo su ogni controller
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private const string ApiKeyHeaderName = "X-Api-Key";

        public ApiKeyMiddleware(RequestDelegate next,IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            var hasApiKeyAttribute = endpoint?.Metadata.GetMetadata<ApiKeyRequiredAttribute>() != null;

            //Apply Middleware check only if the endpoint has the ApiKeyRequiredAttribute
            if (hasApiKeyAttribute)
            {
                var apiKey = httpContext.Request.Headers[ApiKeyHeaderName].FirstOrDefault();

                // Esegui la validazione della chiave API solo se l'endpoint ha l'attributo
                if (apiKey is null || !IsValidApiKey(apiKey!))
                {
                    httpContext.Response.StatusCode = 401; // Unauthorized
                    await httpContext.Response.WriteAsync("Invalid API Key.");
                    return;
                }
            }

            // Passa al prossimo middleware
            await _next(httpContext);
        }

        private bool IsValidApiKey(string apiKey)
        {
            var validApiKey = _configuration.GetValue<string>("ApiKey");
            // Logica di validazione della chiave API
            return apiKey == validApiKey;
        }
    }

}
