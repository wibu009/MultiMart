using System.Text.Json.Serialization;
using MultiMart.Application.Common.FileStorage;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Identity.Users.Update;

public class UpdateUserRequest : IRequest<Unit>
{
    [JsonIgnore]
    public string Id { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public FileUpload? Image { get; set; }
    public bool DeleteCurrentImage { get; set; } = false;
}