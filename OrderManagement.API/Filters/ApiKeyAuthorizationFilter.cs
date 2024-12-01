namespace OrderManagement.API.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using OrderManagement.Common.Authentication;

    public class ApiKeyAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IApiKeyValidator _apiKeyValidator;
        private const string ApiKeyHeaderName = "X-Api-Key";

        public ApiKeyAuthorizationFilter(IApiKeyValidator apiKeyValidator)
        {
            _apiKeyValidator = apiKeyValidator;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Estrarre la chiave API dall'header della richiesta
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Validare la chiave API
            if (!_apiKeyValidator.IsValid(extractedApiKey!))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }

}
