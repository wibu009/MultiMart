using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace BookStack.Infrastructure.ApiVersioning;

public static class Startup
{
    public static IServiceCollection AddApiVersion(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            opt.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("X-Version"),
                new QueryStringApiVersionReader("api-version"),
                new UrlSegmentApiVersionReader());
        });

        services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}