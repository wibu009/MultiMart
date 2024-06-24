using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MultiMart.Infrastructure.OpenApi;

public class SwaggerHeaderAttributeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        object[] attributes = context.MethodInfo.GetCustomAttributes(typeof(SwaggerHeaderAttribute), true);

        foreach (object attr in attributes)
        {
            if (operation.Parameters.Any(p => p.Name == ((SwaggerHeaderAttribute)attr).HeaderName))
            {
                continue;
            }

            if (attr is SwaggerHeaderAttribute attribute)
            {
                var parameter = new OpenApiParameter
                {
                    Name = attribute.HeaderName,
                    In = ParameterLocation.Header,
                    Description = attribute.Description,
                    Required = attribute.IsRequired,
                    Schema = new OpenApiSchema
                    {
                        Type = attribute.Type.ToLower(),
                        Default = new OpenApiString(attribute.DefaultValue ?? string.Empty),
                        Enum = attribute.Enum?.Select(e => new OpenApiString(e) as IOpenApiAny).ToList()
                    }
                };

                operation.Parameters.Add(parameter);
            }
        }
    }
}