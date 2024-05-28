using BookStack.Application;
using BookStack.Host.Configurations;
using BookStack.Host.Controllers;
using BookStack.Infrastructure;
using BookStack.Infrastructure.Common;
using BookStack.Infrastructure.Logging.Serilog;
using Serilog;
using Serilog.Formatting.Compact;

[assembly: ApiConventionType(typeof(ApplicationApiConventions))]

internal class Program
{
    public static async Task Main(string[] args)
    {
        StaticLogger.EnsureInitialized();
        Log.Information("Server Booting Up...");
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddConfigurations().RegisterSerilog();
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