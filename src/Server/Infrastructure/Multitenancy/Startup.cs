using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MultiMart.Application.Multitenancy;
using MultiMart.Infrastructure.Auth;
using MultiMart.Infrastructure.Common.Extensions;
using MultiMart.Infrastructure.Persistence;
using MultiMart.Shared.Authorization;
using MultiMart.Shared.Multitenancy;

namespace MultiMart.Infrastructure.Multitenancy;

internal static class Startup
{
    internal static IServiceCollection AddMultitenancy(this IServiceCollection services)
    {
        return services
            .AddDbContext<TenantDbContext>((p, m) =>
            {
                // TODO: We should probably add specific dbprovider/connectionstring setting for the tenantDb with a fallback to the main databasesettings
                var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                m.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
            })
            .AddMultiTenant<ApplicationTenantInfo>(config =>
            {
                config.Events.OnTenantResolved = async context =>
                {
                    if (context.StoreType != typeof(EFCoreStore<TenantDbContext, ApplicationTenantInfo>))
                    {
                        var sp = ((HttpContext)context.Context!).RequestServices;
                        var store = sp.GetServices<IMultiTenantStore<ApplicationTenantInfo>>()
                            .OfType<EFCoreStore<TenantDbContext, ApplicationTenantInfo>>()
                            .FirstOrDefault();

                        await store!.TryAddAsync((ApplicationTenantInfo)context.TenantInfo!);
                    }
                };
            })
            .WithClaimStrategy(ApplicationClaims.Tenant)
            .WithHeaderStrategy(MultitenancyConstants.TenantIdName)
            .WithQueryStringStrategy(MultitenancyConstants.TenantIdName)
            .WithOAuthStateStrategy()
            .WithDistributedCacheStore(TimeSpan.FromMinutes(60))
            .WithEFCoreStore<TenantDbContext, ApplicationTenantInfo>()
            .Services
            .AddScoped<ITenantService, TenantService>();
    }

    internal static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app) =>
        app.UseMultiTenant();

    private static FinbuckleMultiTenantBuilder<ApplicationTenantInfo> WithQueryStringStrategy(this FinbuckleMultiTenantBuilder<ApplicationTenantInfo> builder, string queryStringKey) =>
        builder.WithDelegateStrategy(context =>
        {
            if (context is not HttpContext httpContext)
            {
                return Task.FromResult((string?)null);
            }

            httpContext.Request.Query.TryGetValue(queryStringKey, out var tenantIdParam);

            return Task.FromResult((string?)tenantIdParam.ToString());
        });

    private static FinbuckleMultiTenantBuilder<ApplicationTenantInfo> WithOAuthStateStrategy(
        this FinbuckleMultiTenantBuilder<ApplicationTenantInfo> builder)
    {
        return builder.WithDelegateStrategy(context =>
        {
            if (context is not HttpContext httpContext)
            {
                return Task.FromResult((string?)null);
            }

            string? state = httpContext.Request.Query["state"];
            if (string.IsNullOrEmpty(state))
            {
                return Task.FromResult((string?)null);
            }

            var stateData = state.FromBase64String<StateData<string>>();
            return Task.FromResult(stateData?.TenantId);
        });
    }
}