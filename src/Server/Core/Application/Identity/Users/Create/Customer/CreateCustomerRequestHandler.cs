namespace MultiMart.Application.Identity.Users.Create.Customer;

public class CreateCustomerRequestHandler : CreateUserRequestHandler<CreateCustomerRequest>
{
    public CreateCustomerRequestHandler(IUserService userService, IStringLocalizer<CreateCustomerRequestHandler> t)
        : base(userService, t)
    {
    }
}