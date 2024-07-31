using System.Text.Json.Serialization;

namespace MultiMart.Application.Identity.Roles.UpdateRolePermission;

public class UpdateRolePermissionsRequest : IRequest<string>
{
    [JsonIgnore]
    public string RoleId { get; set; } = default!;
    public List<string> Permissions { get; set; } = default!;
}