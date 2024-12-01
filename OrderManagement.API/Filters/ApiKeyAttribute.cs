namespace OrderManagement.API.Filters
{
    using Microsoft.AspNetCore.Mvc;

    //Utile per essere usato come decoratore sul singolo controller
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute() : base(typeof(ApiKeyAuthorizationFilter))
        {
        }
    }
}
