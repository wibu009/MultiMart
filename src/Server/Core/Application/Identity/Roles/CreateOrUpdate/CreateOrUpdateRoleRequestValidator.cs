using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Identity.Roles.CreateOrUpdate;

public class CreateOrUpdateRoleRequestValidator : CustomValidator<CreateOrUpdateRoleRequest>
{
    public CreateOrUpdateRoleRequestValidator(IRoleService roleService, IStringLocalizer<CreateOrUpdateRoleRequestValidator> t) =>
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage(t["Role Name is required."])
            .MustAsync(async (role, name, _) => !await roleService.ExistsAsync(name, role.Id))
            .WithMessage(t["Similar Role already exists."]);
}