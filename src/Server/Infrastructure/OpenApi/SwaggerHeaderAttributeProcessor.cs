using System.Reflection;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace MultiMart.Infrastructure.OpenApi;

public class SwaggerHeaderAttributeProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        if (context.MethodInfo?.GetCustomAttribute(typeof(SwaggerHeaderAttribute)) is SwaggerHeaderAttribute attribute)
        {
            var parameters = context.OperationDescription.Operation.Parameters;

            var existingParam = parameters.FirstOrDefault(p =>
                p.Kind == OpenApiParameterKind.Header && p.Name == attribute.HeaderName);
            if (existingParam is not null)
            {
                parameters.Remove(existingParam);
            }

            var schema = new NJsonSchema.JsonSchema
            {
                Type = attribute.Type,
                Default = attribute.DefaultValue,
            };
            if (attribute.Enum is not null)
            {
                foreach (string enumValue in attribute.Enum)
                {
                    schema.Enumeration.Add(enumValue);
                }
            }

            parameters.Add(new OpenApiParameter
            {
                Name = attribute.HeaderName,
                Kind = OpenApiParameterKind.Header,
                Description = attribute.Description,
                IsRequired = attribute.IsRequired,
                Schema = schema,
            });
        }

        return true;
    }
}