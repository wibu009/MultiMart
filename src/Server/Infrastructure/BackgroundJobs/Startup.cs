using MultiMart.Infrastructure.Common;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.MySql;
using Hangfire.PostgreSql;
using Hangfire.SQLite;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiMart.Infrastructure.BackgroundJobs.Hangfire;
using MultiMart.Infrastructure.Persistence;
using Serilog;

namespace MultiMart.Infrastructure.BackgroundJobs;

internal static class Startup
{
    private static readonly ILogger Logger = Log.ForContext(typeof(Startup));

    internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration config)
    {
        services.AddHangfireServer(options => config.GetSection("HangfireSettings:Server").Bind(options));

        services.AddHangfireConsoleExtensions();

        var storageSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        if (storageSettings is null) throw new Exception("Hangfire Storage Provider is not configured.");
        if (string.IsNullOrEmpty(storageSettings.DBProvider)) throw new Exception("Hangfire Storage Provider is not configured.");
        if (string.IsNullOrEmpty(storageSettings.ConnectionString)) throw new Exception("Hangfire Storage Provider ConnectionString is not configured.");
        Logger.Information($"Hangfire: Default Storage Provider: {storageSettings.DBProvider}");
        Logger.Information("For more Hangfire storage, visit https://www.hangfire.io/extensions.html");

        services.AddSingleton<JobActivator, ApplicationJobActivator>();

        services.AddHangfire((provider, hangfireConfig) => hangfireConfig
            .UseDatabase(storageSettings.DBProvider, storageSettings.ConnectionString, config)
            .UseFilter(new ApplicationJobFilter(provider))
            .UseFilter(new LogJobFilter())
            .UseConsole());

        return services;
    }

    private static IGlobalConfiguration UseDatabase(this IGlobalConfiguration hangfireConfig, string dbProvider, string connectionString, IConfiguration config) =>
        dbProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.Npgsql =>
                hangfireConfig.UsePostgreSqlStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<PostgreSqlStorageOptions>()),
            DbProviderKeys.SqlServer =>
                hangfireConfig.UseSqlServerStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<SqlServerStorageOptions>()),
            DbProviderKeys.SqLite =>
                hangfireConfig.UseSQLiteStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<SQLiteStorageOptions>()),
            DbProviderKeys.MySql =>
                hangfireConfig.UseStorage(new MySqlStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<MySqlStorageOptions>())),
            _ => throw new Exception($"Hangfire Storage Provider {dbProvider} is not supported.")
        };

    internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration config)
    {
        var dashboardOptions = config.GetSection("HangfireSettings:Dashboard").Get<DashboardOptions>();
        if (dashboardOptions is null) throw new Exception("Hangfire Dashboard is not configured.");
        dashboardOptions.Authorization = new[]
        {
           new HangfireCustomBasicAuthenticationFilter
           {
                User = config.GetSection("HangfireSettings:Credentials:User").Value!,
                Pass = config.GetSection("HangfireSettings:Credentials:Password").Value!
           }
        };

        return app.UseHangfireDashboard(config["HangfireSettings:Route"], dashboardOptions);
    }
}