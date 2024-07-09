using MultiMart.Application.Common.Validation;
using MultiMart.Application.Identity.Tokens.Requests;

namespace MultiMart.Application.Identity.Tokens.Validations;

public class TokenRequestValidator : CustomValidator<GetTokenRequest>
{
    public TokenRequestValidator(IStringLocalizer<TokenRequestValidator> T)
    {
        RuleFor(p => p.UserNameOrEmail).Cascade(CascadeMode.Stop)
            .Must((request, userNameOrEmail) =>
                !string.IsNullOrEmpty(request.Token) || !string.IsNullOrEmpty(userNameOrEmail))
            .WithMessage(T["Username or Email is required."]);

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
            .MustAsync((request, password, _) => Task.FromResult(!string.IsNullOrEmpty(request.Token) || !string.IsNullOrEmpty(password)))
                .WithMessage(T["Password is required."]);
    }
}