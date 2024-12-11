using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace OrderManagement.API.Extensions
{
    public static class ApiVersioningExtensions
    {
        public static void AddApiVersioningServices(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API V1", Version = "v1" });
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "API V2", Version = "v2" });
            });

        }
    }

   
}
