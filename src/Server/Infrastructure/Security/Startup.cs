using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiMart.Infrastructure.Security.Cors;
using MultiMart.Infrastructure.Security.Header;
using MultiMart.Infrastructure.Security.RateLimit;

namespace MultiMart.Infrastructure.Security;

public static class Startup
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration config)
    {
        services.AddRateLimit(config);
        services.AddAntiforgery();
        services.AddCors();

        return services;
    }

    public static IApplicationBuilder UseSecurity(this IApplicationBuilder app, IConfiguration config)
    {
        app.UseCorsPolicy();
        app.UseSecurityHeaders();
        app.UseRateLimit();

        return app;
    }
}