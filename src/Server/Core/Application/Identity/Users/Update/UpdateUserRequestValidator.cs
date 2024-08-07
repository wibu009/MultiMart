using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Identity.Users.Update;

public class UpdateUserRequestValidator<TUpdateUserRequest> : CustomValidator<TUpdateUserRequest>
    where TUpdateUserRequest : UpdateUserRequest
{
    protected UpdateUserRequestValidator(IUserService userService, IStringLocalizer<TUpdateUserRequest> T)
    {
        RuleFor(p => p.Id)
            .NotEmpty();

        RuleFor(p => p.FirstName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(p => p.LastName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(p => p.Email)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(T["Invalid Email Address."])
            .MustAsync(async (user, email, _) => !await userService.ExistsWithEmailAsync(email, user.Id))
                .WithMessage((_, email) => string.Format(T["Email {0} is already registered."], email));

        RuleFor(p => p.Image);

        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (user, phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!, user.Id))
                .WithMessage((_, phone) => string.Format(T["Phone number {0} is already registered."], phone))
                .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));
    }
}

public class UpdateCustomerRequestValidator : UpdateUserRequestValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator(IUserService userService, IStringLocalizer<UpdateCustomerRequest> T)
        : base(userService, T)
    {
        RuleFor(p => p.LoyaltyPoints)
            .GreaterThanOrEqualTo(0);
    }
}

public class UpdateEmployeeRequestValidator : UpdateUserRequestValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator(IUserService userService, IStringLocalizer<UpdateEmployeeRequest> T)
        : base(userService, T)
    {
        RuleFor(p => p.Position)
            .MaximumLength(75);

        RuleFor(p => p.Department)
            .MaximumLength(75);

        RuleFor(p => p.HireDate)
            .LessThanOrEqualTo(DateTime.Now);

        RuleFor(p => p.ManagerId)
            .NotEmpty();
    }
}