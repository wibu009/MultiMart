using System.Reflection;
using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiMart.Infrastructure.ApiVersioning;
using MultiMart.Infrastructure.Auth;
using MultiMart.Infrastructure.BackgroundJobs;
using MultiMart.Infrastructure.Caching;
using MultiMart.Infrastructure.Common;
using MultiMart.Infrastructure.FileStorage;
using MultiMart.Infrastructure.Localization;
using MultiMart.Infrastructure.Mailing;
using MultiMart.Infrastructure.Mapping;
using MultiMart.Infrastructure.Middleware;
using MultiMart.Infrastructure.Multitenancy;
using MultiMart.Infrastructure.Notifications;
using MultiMart.Infrastructure.OpenApi;
using MultiMart.Infrastructure.Persistence;
using MultiMart.Infrastructure.Persistence.Initialization;
using MultiMart.Infrastructure.Security;
using MultiMart.Infrastructure.Security.Cors;
using MultiMart.Infrastructure.Security.Header;
using MultiMart.Infrastructure.Validations;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace MultiMart.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        var applicationAssembly = typeof(MultiMart.Application.Startup).GetTypeInfo().Assembly;

        services
            .AddHttpContextAccessor()
            .AddApiVersion()
            .AddAuth(config)
            .AddBackgroundJobs(config)
            .AddCaching(config)
            .AddSecurity(config, env)
            .AddExceptionMiddleware()
            .AddBehaviours(applicationAssembly)
            .AddHealthCheck()
            .AddPoLocalization(config)
            .AddFileStorage(config)
            .AddMailing(config)
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddMultitenancy(config)
            .AddNotifications(config)
            .AddOpenApiDocumentation(config)
            .AddPersistence()
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddSettings(config)
            .AddServices();

        MapsterSettings.Configure(services.BuildServiceProvider());

        return services;
    }

    private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
        services.AddHealthChecks().AddCheck<TenantHealthCheck>("Tenant").Services;

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config, IHostEnvironment env) =>
        builder
            .UseRequestLocalization()
            .UseStaticFiles()
            .UseFileStorage()
            .UseSecurity(env)
            .UseExceptionMiddleware()
            .UseRouting()
            .UseApiVersion()
            .UseAuthentication()
            .UseCurrentUser()
            .UseMultiTenancy()
            .UseAuthorization()
            .UseRequestLogging(config)
            .UseHangfireDashboard(config)
            .UseOpenApiDocumentation(config);

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        builder.MapHealthCheck();
        builder.MapNotifications();
        return builder;
    }

    private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapHealthChecks("/api/health");
}