using Figgle;
using Microsoft.Extensions.Options;
using MultiMart.Infrastructure.Common;
using MultiMart.Application;
using MultiMart.Host.Configurations;
using MultiMart.Host.Controllers;
using MultiMart.Infrastructure;
using MultiMart.Infrastructure.Logging;
using MultiMart.Infrastructure.Logging.Serilog;
using Serilog;

[assembly: ApiConventionType(typeof(ApplicationApiConventions))]

namespace MultiMart.Host;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        StaticLogger.EnsureInitialized();
        Log.Information("Server Booting Up...");
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddConfigurations().RegisterSerilog();
            var loggerSettings = builder.Services.BuildServiceProvider()
                .GetRequiredService<IOptions<LoggerSettings>>().Value;
            Console.WriteLine(FiggleFonts.Standard.Render(loggerSettings.AppName));
            builder.Services.AddControllers();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            var app = builder.Build();

            await app.Services.InitializeDatabasesAsync().ConfigureAwait(false);

            app.UseInfrastructure(builder.Configuration);
            app.MapEndpoints();
            await app.RunAsync().ConfigureAwait(false);
        }
        catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
        {
            StaticLogger.EnsureInitialized();
            Log.Fatal(ex, "Unhandled exception");
        }
        finally
        {
            StaticLogger.EnsureInitialized();
            Log.Information("Server Shutting down...");
            await Log.CloseAndFlushAsync().ConfigureAwait(false);
        }
    }
}