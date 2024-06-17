using Microsoft.AspNetCore.Authorization;
using MultiMart.Shared.Authorization;

namespace MultiMart.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    // MustHavePermission("Create", "Products")
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = ApplicationPermission.NameFor(action, resource);

    // MustHavePermission("Create", new[] { "Products", "Categories" })
    public MustHavePermissionAttribute(string[] actions, string resource)
    {
        foreach (string action in actions)
        {
            Policy += ApplicationPermission.NameFor(action, resource) + ";";
        }
    }

    // MustHavePermission(new[] { "Create", "Update" }, Products)
    public MustHavePermissionAttribute(string action, string[] resources)
    {
        foreach (string resource in resources)
        {
            Policy += ApplicationPermission.NameFor(action, resource) + ";";
        }
    }

    // MustHavePermission(new[] { "Create", "Update" }, new[] { "Products", "Categories" })
    public MustHavePermissionAttribute(string[] actions, string[] resources)
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