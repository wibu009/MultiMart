namespace MultiMart.Application.Identity.Users.Create.Employee;

public class CreateEmployeeRequestValidator : CreateUserRequestValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator(IUserService userService, IStringLocalizer<CreateEmployeeRequest> T)
        : base(userService, T)
    {
        RuleFor(p => p.Position).Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(p => p.Department).Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(p => p.HireDate).Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}