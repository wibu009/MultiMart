using System.Net;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiMart.Infrastructure.Caching;

namespace MultiMart.Infrastructure.Security.RateLimit;

public static class Register
{
    public static IServiceCollection AddRateLimit(this IServiceCollection services, IConfiguration config)
    {
        var cacheSettings = config.GetSection(nameof(CacheSettings)).Get<CacheSettings>();
        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = false;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = (int)HttpStatusCode.TooManyRequests;
            options.RealIpHeader = "X-Real-IP";
            options.ClientIdHeader = "X-ClientId";
            options.GeneralRules = new List<RateLimitRule>
            {
                new()
                {
                    Endpoint = "*",
                    Limit = 3000,
                    Period = "1m",
                    QuotaExceededResponse = new QuotaExceededResponse()
                    {
                        Content = "Too many requests, please try again later after 1 minute.",
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.TooManyRequests
                    },
                    MonitorMode = false,
                },
            };
        });

        if (cacheSettings?.UseDistributedCache == true)
        {
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
            services.AddDistributedRateLimiting();
        }
        else
        {
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddInMemoryRateLimiting();
        }

        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        return services;
    }

    public static IApplicationBuilder UseRateLimit(this IApplicationBuilder app)
    {
        app.UseIpRateLimiting();
        return app;
    }
}