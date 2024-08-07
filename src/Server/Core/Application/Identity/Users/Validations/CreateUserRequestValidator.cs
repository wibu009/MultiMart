using MultiMart.Application.Common.Validation;
using MultiMart.Application.Identity.Users.Interfaces;
using MultiMart.Application.Identity.Users.Requests.Commands;

namespace MultiMart.Application.Identity.Users.Validations;

public class CreateUserRequestValidator : CustomValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(IUserService userService, IStringLocalizer<CreateUserRequestValidator> T)
    {
        RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(T["Invalid Email Address."])
            .MustAsync(async (email, _) => !await userService.ExistsWithEmailAsync(email))
                .WithMessage((_, email) => T["Email {0} is already registered.", email]);

        RuleFor(u => u.UserName).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6)
            .MustAsync(async (name, _) => !await userService.ExistsWithNameAsync(name))
                .WithMessage((_, name) => T["Username {0} is already taken.", name]);

        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!))
                .WithMessage((_, phone) => T["Phone number {0} is already registered.", phone!])
                .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));

        RuleFor(p => p.FirstName).Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(p => p.LastName).Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]+").WithMessage(T["Password must contain at least one uppercase letter."])
            .Matches("[a-z]+").WithMessage(T["Password must contain at least one lowercase letter."])
            .Matches("[0-9]+").WithMessage(T["Password must contain at least one number."])
            .Matches("[!@#$%^&*]+").WithMessage(T["Password must contain at least one special character."]);

        RuleFor(p => p.ConfirmPassword).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Equal(p => p.Password);
    }
}