using MultiMart.Application.Auditing;
using MultiMart.Application.Auditing.Get;
using MultiMart.Application.Identity.Users;
using MultiMart.Application.Identity.Users.ChangePassword;
using MultiMart.Application.Identity.Users.Get;
using MultiMart.Application.Identity.Users.GetUserPermissions;
using MultiMart.Application.Identity.Users.Update;
using MultiMart.Infrastructure.Common.Extensions;

namespace MultiMart.Host.Controllers.Personal;

public class PersonalController : VersionNeutralApiController
{
    [HttpGet("profile")]
    [SwaggerOperation("Get profile details of currently logged in user.", "")]
    public async Task<UserDetailsDto> GetProfileAsync(CancellationToken cancellationToken)
        => await Mediator.Send(new GetUserRequest(User.GetUserId() ?? string.Empty), cancellationToken);

    [HttpPut("profile")]
    [SwaggerOperation("Update profile details of currently logged in user.", "")]
    public async Task<ActionResult> UpdateProfileAsync(UpdateUserRequest request)
        => Ok(await Mediator.Send(request.SetPropertyValue(nameof(UpdateUserRequest.Id), User.GetUserId() ?? string.Empty)));

    [HttpPut("change-password")]
    [SwaggerOperation("Change password of currently logged in user.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Register))]
    public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest model)
        => Ok(await Mediator.Send(model));

    [HttpGet("permissions")]
    [SwaggerOperation("Get permissions of currently logged in user.", "")]
    public async Task<List<string>> GetPermissionsAsync(CancellationToken cancellationToken)
        => await Mediator.Send(new GetUserPermissionsRequest(User.GetUserId() ?? string.Empty), cancellationToken);

    [HttpGet("logs")]
    [SwaggerOperation("Get audit logs of currently logged in user.", "")]
    public Task<List<AuditDto>> GetLogsAsync()
        => Mediator.Send(new GetMyAuditLogsRequest());
}