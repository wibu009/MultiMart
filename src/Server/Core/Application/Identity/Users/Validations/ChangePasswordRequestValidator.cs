using MultiMart.Application.Common.Validation;
using MultiMart.Application.Identity.Users.Requests.Commands;

namespace MultiMart.Application.Identity.Users.Validations;

public class ChangePasswordRequestValidator : CustomValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator(IStringLocalizer<ChangePasswordRequestValidator> T)
    {
        RuleFor(p => p.Password)
            .NotEmpty();

        RuleFor(p => p.NewPassword)
            .NotEmpty();

        RuleFor(p => p.ConfirmNewPassword)
            .Equal(p => p.NewPassword)
            .WithMessage(T["Passwords do not match."]);
    }
}