using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Uri = System.Uri;

namespace MultiMart.Infrastructure.OpenApi;

internal static class Startup
{
    internal static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
        if (settings == null) return services;
        if (settings.Enable)
        {
            services.AddVersionedApiExplorer(o => o.SubstituteApiVersionInUrl = true);
            services.AddEndpointsApiExplorer();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = settings.Title,
                        Version = description.GroupName,
                        Description = settings.Description,
                        Contact = new OpenApiContact
                        {
                            Name = settings.ContactName,
                            Email = settings.ContactEmail,
                            Url = new Uri(settings.ContactUrl ?? string.Empty)
                        },
                        License = new OpenApiLicense
                        {
                            Name = settings.LicenseName,
                            Url = new Uri(settings.LicenseUrl ?? string.Empty)
                        }
                    });

                    // Check if the security definition already exists
                    if (!options.SwaggerGeneratorOptions.SecuritySchemes.ContainsKey(JwtBearerDefaults.AuthenticationScheme))
                    {
                        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Description = "Input your Bearer token to access this API",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                            BearerFormat = "JWT",
                        });
                    }

                    options.EnableAnnotations();
                    options.OperationFilter<SwaggerGlobalAuthOperationFilter>();
                    options.OperationFilter<SwaggerGlobalAcceptLanguageHeaderOperationFilter>();
                    options.OperationFilter<SwaggerHeaderAttributeOperationFilter>();
                });
            }
        }

        return services;
    }

    internal static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app, IConfiguration config)
    {
        var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
        if (config.GetValue<bool>("SwaggerSettings:Enable"))
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                }
            });
        }

        return app;
    }
}