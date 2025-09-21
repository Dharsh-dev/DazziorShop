using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            // 👇 This is the magic line
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"Dazzior API {description.ApiVersion}", // 👈 Custom title
                Version = description.ApiVersion.ToString(),
                Description = "Your API description here"
            });
        }
    }
}