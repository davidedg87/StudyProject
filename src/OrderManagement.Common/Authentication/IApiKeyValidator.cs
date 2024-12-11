namespace OrderManagement.Common.Authentication
{
    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}
