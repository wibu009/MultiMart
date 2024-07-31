using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Identity.Roles.UpdateRolePermission;

public class UpdateRolePermissionsRequestValidator : CustomValidator<UpdateRolePermissionsRequest>
{
    public UpdateRolePermissionsRequestValidator()
    {
        RuleFor(r => r.Permissions)
            .NotNull();
    }
}