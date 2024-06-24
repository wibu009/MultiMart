using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MultiMart.Infrastructure.OpenApi;

public class SwaggerGlobalAuthOperationFilter : IOperationFilter
{
    private readonly string _name;

    public SwaggerGlobalAuthOperationFilter()
        : this("Bearer")
    {
    }

    public SwaggerGlobalAuthOperationFilter(string name)
    {
        _name = name;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        if (apiDescription.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;
        bool isAllowAnonymous = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
            .Union(controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(true))
            .OfType<AllowAnonymousAttribute>().Any();

        if (!isAllowAnonymous)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = _name } };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }
}