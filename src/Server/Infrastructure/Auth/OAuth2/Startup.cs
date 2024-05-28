using BookStack.Infrastructure.Auth.OAuth2.Facebook;
using BookStack.Infrastructure.Auth.OAuth2.Google;
using Microsoft.Extensions.DependencyInjection;

namespace BookStack.Infrastructure.Auth.OAuth2;

public static class Startup
{
    public static void AddOAuth2(this IServiceCollection services)
    {
        services.AddOptions<GoogleSettings>()
            .BindConfiguration($"SecuritySettings:{nameof(GoogleSettings)}")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<FacebookSettings>()
            .BindConfiguration($"SecuritySettings:{nameof(FacebookSettings)}")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<OAuth2Service>();
    }
}