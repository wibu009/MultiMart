using BookStack.Infrastructure.Mailing.SendGrid;
using BookStack.Infrastructure.Mailing.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStack.Infrastructure.Mailing;

internal static class Startup
{
    internal static IServiceCollection AddMailing(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<SmtpMailSettings>(config.GetSection(nameof(SmtpMailSettings)));
        services.Configure<SendGridMailSettings>(config.GetSection(nameof(SendGridMailSettings)));

        return services;
    }
}