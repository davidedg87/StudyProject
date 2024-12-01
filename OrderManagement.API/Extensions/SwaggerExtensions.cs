using Asp.Versioning.ApiExplorer;

namespace OrderManagement.API.Extensions
{
    public static class SwaggerExtensions
    {

        public static void AddSwaggerServices(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    apiVersionDescriptionProvider!.ApiVersionDescriptions.ToList()
                      .ForEach(version => options.SwaggerEndpoint($"/swagger/{version.GroupName}/swagger.json", version.GroupName.ToUpperInvariant()));

                });
            }

        }
    }
}
