using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Identity.Users.ResetPassword;

public class ResetPasswordRequestValidator : CustomValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator(IStringLocalizer<ResetPasswordRequestValidator> T)
    {
        RuleFor(p => p.UserId)
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
            .Equal(p => p.Password)
            .WithMessage(T["Passwords do not match."]);

        RuleFor(p => p.Token)
            .NotEmpty();
    }
}