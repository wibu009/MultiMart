using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Identity.Roles.Get;

public class GetRoleRequestValidator : CustomValidator<GetRoleRequest>
{
    public GetRoleRequestValidator(
        IRoleService roleService,
        IStringLocalizer<GetRoleRequestValidator> t)
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage(t["Role Id is required."])
            .MustAsync(async (role, name, _) => !await roleService.ExistsAsync(name, role.Id))
            .WithMessage(t["Role does not exist."]);

    }
}