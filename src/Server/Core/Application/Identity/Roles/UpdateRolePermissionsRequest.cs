using System.Text.Json.Serialization;
using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Identity.Roles;

public class UpdateRolePermissionsRequest
{
    [JsonIgnore]
    public string RoleId { get; set; } = default!;
    public List<string> Permissions { get; set; } = default!;
}

public class UpdateRolePermissionsRequestValidator : CustomValidator<UpdateRolePermissionsRequest>
{
    public UpdateRolePermissionsRequestValidator()
    {
        RuleFor(r => r.Permissions)
            .NotNull();
    }
}