using Microsoft.AspNetCore.Authorization;
using MultiMart.Shared.Authorization;

namespace MultiMart.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = ApplicationPermission.NameFor(action, resource);
}