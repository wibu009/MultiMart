using MultiMart.Application.Common.Validation;
using MultiMart.Application.Identity.Roles.Interfaces;
using MultiMart.Application.Identity.Roles.Requests.Commands;

namespace MultiMart.Application.Identity.Roles.Validations;

public class CreateOrUpdateRoleRequestValidator : CustomValidator<CreateOrUpdateRoleRequest>
{
    public CreateOrUpdateRoleRequestValidator(IRoleService roleService, IStringLocalizer<CreateOrUpdateRoleRequestValidator> T) =>
        RuleFor(r => r.Name)
            .NotEmpty()
            .MustAsync(async (role, name, _) => !await roleService.ExistsAsync(name, role.Id))
            .WithMessage(T["Similar Role already exists."]);
}