using Microsoft.Extensions.DependencyInjection;
using MultiMart.Infrastructure.Auth.OAuth.Facebook;
using MultiMart.Infrastructure.Auth.OAuth.Google;

namespace MultiMart.Infrastructure.Auth.OAuth;

public static class Startup
{
    public static void AddOAuth(this IServiceCollection services)
    {
        services.AddOptions<GoogleSettings>()
            .BindConfiguration($"SecuritySettings:{nameof(GoogleSettings)}")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<FacebookSettings>()
            .BindConfiguration($"SecuritySettings:{nameof(FacebookSettings)}")
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}