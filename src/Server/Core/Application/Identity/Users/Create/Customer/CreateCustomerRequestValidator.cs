namespace MultiMart.Application.Identity.Users.Create.Customer;

public class CreateCustomerRequestValidator : CreateUserRequestValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator(IUserService userService, IStringLocalizer<CreateCustomerRequest> T)
        : base(userService, T)
    {
        RuleFor(p => p.LoyaltyPoints).Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(0);
    }
}