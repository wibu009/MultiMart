using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.Mailing;
using MultiMart.Infrastructure.Common;
using MultiMart.Shared.Multitenancy;

namespace MultiMart.Infrastructure.Identity.User;

internal partial class UserService
{
    public async Task<string> ForgotPasswordAsync(string email, string? origin, CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        // Find by email or username
        var user = await _userManager.FindByEmailAsync(email.Normalize()!);
        if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            // Don't reveal that the user does not exist or is not confirmed
            throw new InternalServerException(_t["An Error has occurred!"]);
        }

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        string passwordResetUri = await GetEmailForgotPasswordUriAsync(user, origin!);
        var emailModel = new UserEmailTemplateModel()
        {
            Email = user.Email!,
            UserName = user.UserName!,
            Url = passwordResetUri
        };
        var mailRequest = new MailRequest(
            new List<string> { email },
            _t["Reset Password"],
            _templateService.GenerateEmailTemplate("email-reset-password", emailModel));
        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, cancellationToken));

        return _t["Password Reset Mail has been sent to your authorized Email."];
    }

    public async Task<string> ResetPasswordAsync(string userId, string? password, string? token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        // Don't reveal that the user does not exist
        _ = user ?? throw new InternalServerException(_t["An Error has occurred!"]);

        string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token!));
        var result = await _userManager.ResetPasswordAsync(user, code, password!);

        return result.Succeeded
            ? _t["Password Reset Successful!"]
            : throw new InternalServerException(_t["An Error has occurred!"]);
    }

    public async Task ChangePasswordAsync(string password, string newPassword, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        var result = await _userManager.ChangePasswordAsync(user, password, newPassword);

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Change password failed"], result.GetErrors(_t));
        }
    }

    private async Task<string> GetEmailForgotPasswordUriAsync(ApplicationUser user, string origin)
    {
        EnsureValidTenant();

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        const string route = "account/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        string passwordResetUri = QueryHelpers.AddQueryString(endpointUri.ToString(), MultitenancyConstants.TenantIdName, _currentTenant.Id!);
        passwordResetUri = QueryHelpers.AddQueryString(passwordResetUri, QueryStringKeys.UserId, user.Id);
        passwordResetUri = QueryHelpers.AddQueryString(passwordResetUri, QueryStringKeys.Token, token);
        return passwordResetUri;
    }
}