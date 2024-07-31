using System.Text.Json.Serialization;

namespace MultiMart.Application.Identity.Users.ForgotPassword;

public class ForgotPasswordRequest : IRequest<string>
{
    public string Email { get; set; } = default!;
    [JsonIgnore]
    public string? Origin { get; set; } = default!;
}