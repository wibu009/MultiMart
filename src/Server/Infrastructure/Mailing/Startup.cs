using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiMart.Infrastructure.Mailing.SendGrid;
using MultiMart.Infrastructure.Mailing.Smtp;

namespace MultiMart.Infrastructure.Mailing;

internal static class Startup
{
    internal static IServiceCollection AddMailing(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<SmtpMailSettings>(config.GetSection(nameof(SmtpMailSettings)));
        services.Configure<SendGridMailSettings>(config.GetSection(nameof(SendGridMailSettings)));

        return services;
    }
}