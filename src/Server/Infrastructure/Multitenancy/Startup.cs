using BookStack.Application.Multitenancy;
using BookStack.Infrastructure.Auth.OAuth2;
using BookStack.Infrastructure.Common.Extensions;
using BookStack.Infrastructure.Persistence;
using BookStack.Infrastructure.Security.Encrypt;
using BookStack.Shared.Authorization;
using BookStack.Shared.Multitenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace BookStack.Infrastructure.Multitenancy;

internal static class Startup
{
    internal static IServiceCollection AddMultitenancy(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<TenantDbContext>((p, m) =>
            {
                // TODO: We should probably add specific dbprovider/connectionstring setting for the tenantDb with a fallback to the main databasesettings
                var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                m.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
            })
            .AddMultiTenant<ApplicationTenantInfo>()
                .WithClaimStrategy(ApplicationClaims.Tenant)
                .WithHeaderStrategy(MultitenancyConstants.TenantIdName)
                .WithQueryStringStrategy(MultitenancyConstants.TenantIdName)
                .WithOAuthStateStrategy(configuration)
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

            httpContext.Request.Query.TryGetValue(queryStringKey, out StringValues tenantIdParam);

            return Task.FromResult((string?)tenantIdParam.ToString());
        });

    private static FinbuckleMultiTenantBuilder<ApplicationTenantInfo> WithOAuthStateStrategy(
        this FinbuckleMultiTenantBuilder<ApplicationTenantInfo> builder, IConfiguration configuration)
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

            var encryptionSettings = configuration.GetSection(nameof(EncryptionSettings)).Get<EncryptionSettings>();
            var stateData = state.Decrypt<StateData<AuthStateData>>(encryptionSettings.Key, encryptionSettings.IV);
            return Task.FromResult(stateData?.TenantId);
        });
    }
}