namespace MultiMart.Application.Identity.Users.ConfirmPhoneNumber;

public class ConfirmPhoneNumberRequest : IRequest<string>
{
    public string Token { get; set; }
    public string UserId { get; set; }

    public ConfirmPhoneNumberRequest(string token, string userId)
    {
        Token = token;
        UserId = userId;
    }
}