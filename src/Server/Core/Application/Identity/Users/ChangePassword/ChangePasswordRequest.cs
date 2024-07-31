using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Identity.Users.ChangePassword;

public class ChangePasswordRequest : IRequest
{
    public string Password { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmNewPassword { get; set; } = default!;
}