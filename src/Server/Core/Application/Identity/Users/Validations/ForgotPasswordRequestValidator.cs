using MultiMart.Application.Common.Validation;
using MultiMart.Application.Identity.Users.Requests.Commands;

namespace MultiMart.Application.Identity.Users.Validations;

public class ForgotPasswordRequestValidator : CustomValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> T) =>
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .WithMessage(T["Invalid Email Address."]);
}