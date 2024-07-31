namespace MultiMart.Application.Identity.Users.ConfirmEmail;

public class ConfirmEmailRequest : IRequest<string>
{
    public string Token { get; set; }
    public string UserId { get; set; }

    public ConfirmEmailRequest(string token, string userId)
    {
        Token = token;
        UserId = userId;
    }
}