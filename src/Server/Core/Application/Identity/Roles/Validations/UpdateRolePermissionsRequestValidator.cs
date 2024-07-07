using MultiMart.Application.Common.Validation;
using MultiMart.Application.Identity.Roles.Requests.Commands;

namespace MultiMart.Application.Identity.Roles.Validations;

public class UpdateRolePermissionsRequestValidator : CustomValidator<UpdateRolePermissionsRequest>
{
    public UpdateRolePermissionsRequestValidator()
    {
        RuleFor(r => r.Permissions)
            .NotNull();
    }
}