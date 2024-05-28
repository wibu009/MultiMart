namespace MultiMart.Infrastructure.BackgroundJobs.Hangfire;

public class HangfireStorageSettings
{
    public string? StorageProvider { get; set; }
    public string? ConnectionString { get; set; }
}