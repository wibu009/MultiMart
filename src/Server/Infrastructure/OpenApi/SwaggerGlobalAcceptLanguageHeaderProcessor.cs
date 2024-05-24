using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace BookStack.Infrastructure.OpenApi;

public class SwaggerGlobalAcceptLanguageHeaderProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        var parameters = context?.OperationDescription.Operation.Parameters;

        var existingParam = parameters?.FirstOrDefault(p =>
            p.Kind == OpenApiParameterKind.Header && p.Name == "Accept-Language");
        if (existingParam is not null)
        {
            parameters.Remove(existingParam);
        }

        var schema = new NJsonSchema.JsonSchema
        {
            Type = JsonObjectType.String,
            Default = "en-US",
        };
        string[] enumValues = { "en-US", "en", "fr", "fr-FR", "de", "de-DE", "it", "it-IT", "pt", "nl", "nl-NL", "vi", "vi-VN" };
        foreach (string enumValue in enumValues)
        {
            schema.Enumeration.Add(enumValue);
        }

        parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            Kind = OpenApiParameterKind.Header,
            Description = "Language support (English and Vietnamese)",
            IsRequired = false,
            Schema = schema,
        });

        return true;
    }
}