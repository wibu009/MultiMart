using BookStack.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace BookStack.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = ApplicationPermission.NameFor(action, resource);
}