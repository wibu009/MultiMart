namespace MultiMart.Application.Identity.Users.ResetPassword;

public class ResetPasswordRequest : IRequest<string>
{
    public required string UserId { get; set; }

    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }

    public string? Token { get; set; }
}