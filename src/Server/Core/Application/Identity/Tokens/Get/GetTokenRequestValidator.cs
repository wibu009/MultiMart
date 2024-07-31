using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Identity.Tokens.Get;

public class GetTokenRequestValidator : CustomValidator<GetTokenRequest>
{
    public GetTokenRequestValidator(IStringLocalizer<GetTokenRequestValidator> T)
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