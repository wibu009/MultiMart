using System.Text.Json.Serialization;

namespace MultiMart.Application.Identity.Users.SetUserRole;

public class SetUserRolesRequest : IRequest<string>
{
    [JsonIgnore]
    public string UserId { get; set; } = default!;
    public List<UserRoleDto> UserRoles { get; set; } = new();
}
