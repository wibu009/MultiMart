using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.Mailing;
using MultiMart.Application.Identity.Users.Password;
using MultiMart.Infrastructure.Common;
using MultiMart.Shared.Multitenancy;

namespace MultiMart.Infrastructure.Identity.User;

internal partial class UserService
{
    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        EnsureValidTenant();

        //Find by email or username
        var user = await _userManager.FindByEmailAsync(request.Email.Normalize()!);
        if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            // Don't reveal that the user does not exist or is not confirmed
            throw new InternalServerException(_t["An Error has occurred!"]);
        }

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        string passwordResetUri = await GetEmailForgotPasswordUriAsync(user, origin);
        var emailModel = new UserEmailTemplateModel()
        {
            Email = user.Email!,
            UserName = user.UserName!,
            Url = passwordResetUri
        };
        var mailRequest = new MailRequest(
            new List<string> { request.Email },
            _t["Reset Password"],
            _templateService.GenerateEmailTemplate("email-reset-password", emailModel));
        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));

        return _t["Password Reset Mail has been sent to your authorized Email."];
    }

    public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        // Don't reveal that the user does not exist
        _ = user ?? throw new InternalServerException(_t["An Error has occurred!"]);

        string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token!));
        var result = await _userManager.ResetPasswordAsync(user, code, request.Password!);

        return result.Succeeded
            ? _t["Password Reset Successful!"]
            : throw new InternalServerException(_t["An Error has occurred!"]);
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

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