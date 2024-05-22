namespace BookStack.Host.Configurations;

internal static class Startup
{
    internal static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
    {
        const string configurationsDirectory = "Configurations";
        var configFiles = Directory.GetFiles(configurationsDirectory, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();

        builder.Configuration.AddConfigurationFiles(configurationsDirectory, configFiles)
            .AddEnvironmentVariables();
        return builder;
    }
}

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddConfigurationFiles(this IConfigurationBuilder builder, string configurationsDirectory, List<string?> configFiles)
    {
        foreach (string? file in configFiles)
        {
            builder.AddJsonFile($"{configurationsDirectory}/{file}.json", optional: false, reloadOnChange: true);
        }

        return builder;
    }
}