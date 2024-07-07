using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiMart.Infrastructure.Security.Cors;
using MultiMart.Infrastructure.Security.Header;
using MultiMart.Infrastructure.Security.RateLimit;

namespace MultiMart.Infrastructure.Security;

public static class Startup
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            services.AddCorsPolicy();
            return services;
        }

        services.AddRateLimit(config);
        services.AddAntiforgery();
        services.AddCorsPolicy(config);
        return services;
    }

    public static IApplicationBuilder UseSecurity(this IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseCorsPolicy();
            return app;
        }

        app.UseCorsPolicy();
        app.UseSecurityHeaders();
        app.UseRateLimit();

        return app;
    }
}