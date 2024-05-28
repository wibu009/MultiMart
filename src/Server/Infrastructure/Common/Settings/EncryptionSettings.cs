namespace BookStack.Infrastructure.Security.Encrypt;

public class EncryptionSettings
{
    public string Key { get; set; } = default!;
    public string IV { get; set; } = default!;
}