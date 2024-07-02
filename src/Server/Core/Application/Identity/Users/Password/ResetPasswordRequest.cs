namespace MultiMart.Application.Identity.Users.Password;

public class ResetPasswordRequest
{
    public required string UserId { get; set; }

    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }

    public string? Token { get; set; }
}