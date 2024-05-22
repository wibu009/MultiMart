namespace BookStack.Application.Identity.Tokens;

public class TokenRequest
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
}

public class TokenRequestValidator : CustomValidator<TokenRequest>
{
    public TokenRequestValidator(IStringLocalizer<TokenRequestValidator> T)
    {
        RuleFor(p => p.UserNameOrEmail).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(T["Invalid Email Address."]);

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}