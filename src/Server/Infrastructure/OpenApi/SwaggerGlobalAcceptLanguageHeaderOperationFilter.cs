using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MultiMart.Infrastructure.OpenApi;

public class SwaggerGlobalAcceptLanguageHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var existingParam = operation.Parameters?.FirstOrDefault(p =>
            p.In == ParameterLocation.Header && p.Name == "Accept-Language");
        if (existingParam is not null)
        {
            operation.Parameters.Remove(existingParam);
        }

        var schema = new OpenApiSchema
        {
            Type = "string",
            Default = new OpenApiString("en-US"),
            Enum = new List<IOpenApiAny>
            {
                new OpenApiString("en-US"),
                new OpenApiString("en"),
                new OpenApiString("fr"),
                new OpenApiString("fr-FR"),
                new OpenApiString("de"),
                new OpenApiString("de-DE"),
                new OpenApiString("it"),
                new OpenApiString("it-IT"),
                new OpenApiString("pt"),
                new OpenApiString("nl"),
                new OpenApiString("nl-NL"),
                new OpenApiString("vi"),
                new OpenApiString("vi-VN")
            }
        };

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Description = "Language support (English, French, German, Italian, Portuguese, Dutch, Vietnamese)",
            Required = false,
            Schema = schema,
        });
    }
}