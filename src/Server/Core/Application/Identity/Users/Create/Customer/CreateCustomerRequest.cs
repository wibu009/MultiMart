namespace MultiMart.Application.Identity.Users.Create.Customer;

public class CreateCustomerRequest : CreateUserRequest
{
    public int LoyaltyPoints { get; set; }
}