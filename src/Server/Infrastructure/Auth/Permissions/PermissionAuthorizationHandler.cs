using Microsoft.AspNetCore.Authorization;
using MultiMart.Application.Identity.Users;
using MultiMart.Application.Identity.Users.Interfaces;
using MultiMart.Shared.Authorization;

namespace MultiMart.Infrastructure.Auth.Permissions;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserService _userService;

    public PermissionAuthorizationHandler(IUserService userService) =>
        _userService = userService;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User?.GetUserId() is { } userId)
        {
            string[] permissions = requirement.Permission.Split(';');
            foreach (string permission in permissions)
            {
                if (await _userService.HasPermissionAsync(userId, permission))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}