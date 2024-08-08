using System.Text.Json.Serialization;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Identity.Users.Create;

public class CreateUserRequest : IRequest<string>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    [JsonIgnore]
    public string? Origin { get; set; }
}