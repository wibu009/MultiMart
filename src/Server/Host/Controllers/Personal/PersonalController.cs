using MultiMart.Application.Auditing;
using MultiMart.Application.Auditing.Dtos;
using MultiMart.Application.Auditing.Request.Queries;
using MultiMart.Application.Identity.Users;
using MultiMart.Application.Identity.Users.Password;
using MultiMart.Shared.Authorization;

namespace MultiMart.Host.Controllers.Personal;

public class PersonalController : VersionNeutralApiController
{
    private readonly IUserService _userService;

    public PersonalController(IUserService userService) => _userService = userService;

    [HttpGet("profile")]
    [SwaggerOperation("Get profile details of currently logged in user.", "")]
    public async Task<ActionResult<UserDetailsDto>> GetProfileAsync(CancellationToken cancellationToken)
    {
        return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await _userService.GetAsync(userId, cancellationToken));
    }

    [HttpPut("profile")]
    [SwaggerOperation("Update profile details of currently logged in user.", "")]
    public async Task<ActionResult> UpdateProfileAsync(UpdateUserRequest request)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _userService.UpdateAsync(request, userId);
        return Ok();
    }

    [HttpPut("change-password")]
    [SwaggerOperation("Change password of currently logged in user.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Register))]
    public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest model)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _userService.ChangePasswordAsync(model, userId);
        return Ok();
    }

    [HttpGet("permissions")]
    [SwaggerOperation("Get permissions of currently logged in user.", "")]
    public async Task<ActionResult<List<string>>> GetPermissionsAsync(CancellationToken cancellationToken)
    {
        return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await _userService.GetPermissionsAsync(userId, cancellationToken));
    }

    [HttpGet("logs")]
    [SwaggerOperation("Get audit logs of currently logged in user.", "")]
    public Task<List<AuditDto>> GetLogsAsync()
    {
        return Mediator.Send(new GetMyAuditLogsRequest());
    }
}