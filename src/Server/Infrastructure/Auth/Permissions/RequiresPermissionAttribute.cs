using Microsoft.AspNetCore.Authorization;
using MultiMart.Shared.Authorization;

namespace MultiMart.Infrastructure.Auth.Permissions;

public class RequiresPermissionAttribute : AuthorizeAttribute
{
    // RequiresPermission("Create", "Products")
    public RequiresPermissionAttribute(string action, string resource) =>
        Policy = ApplicationPermission.NameFor(action, resource);

    // RequiresPermission("Create", new[] { "Products", "Categories" })
    public RequiresPermissionAttribute(string[] actions, string resource)
    {
        foreach (string action in actions)
        {
            Policy += ApplicationPermission.NameFor(action, resource) + ";";
        }
    }

    // RequiresPermission(new[] { "Create", "Update" }, Products)
    public RequiresPermissionAttribute(string action, string[] resources)
    {
        foreach (string resource in resources)
        {
            Policy += ApplicationPermission.NameFor(action, resource) + ";";
        }
    }

    // MustHavePermission(new[] { "Create", "Update" }, new[] { "Products", "Categories" })
    public RequiresPermissionAttribute(string[] actions, string[] resources)
    {
        foreach (string action in actions)
        {
            foreach (string resource in resources)
            {
                Policy += ApplicationPermission.NameFor(action, resource) + ";";
            }
        }
    }
}