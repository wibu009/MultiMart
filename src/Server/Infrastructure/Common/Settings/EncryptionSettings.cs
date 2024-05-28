namespace MultiMart.Infrastructure.Common.Settings;

public class EncryptionSettings
{
    public string Key { get; set; } = default!;
    public string IV { get; set; } = default!;
}