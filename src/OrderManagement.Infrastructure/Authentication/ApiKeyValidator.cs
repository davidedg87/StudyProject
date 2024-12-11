using Microsoft.Extensions.Configuration;
using OrderManagement.Common.Authentication;

namespace OrderManagement.Infrastructure.Authentication
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly IConfiguration _configuration;

        public ApiKeyValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsValid(string apiKey)
        {
            // Recupera la chiave API valida dal configuration o database
            var validApiKey = _configuration.GetValue<string>("ApiKey");
            return apiKey == validApiKey;
        }
    }
}
