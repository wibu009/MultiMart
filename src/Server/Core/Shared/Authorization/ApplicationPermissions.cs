using System.Collections.ObjectModel;

namespace BookStack.Shared.Authorization;

public static class ApplicationAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class ApplicationResource
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);
}

public static class ApplicationPermissions
{
    private static readonly ApplicationPermission[] _all = new ApplicationPermission[]
    {
        new("View Dashboard", ApplicationAction.View, ApplicationResource.Dashboard),
        new("View Hangfire", ApplicationAction.View, ApplicationResource.Hangfire),
        new("View Users", ApplicationAction.View, ApplicationResource.Users),
        new("Search Users", ApplicationAction.Search, ApplicationResource.Users),
        new("Create Users", ApplicationAction.Create, ApplicationResource.Users),
        new("Update Users", ApplicationAction.Update, ApplicationResource.Users),
        new("Delete Users", ApplicationAction.Delete, ApplicationResource.Users),
        new("Export Users", ApplicationAction.Export, ApplicationResource.Users),
        new("View UserRoles", ApplicationAction.View, ApplicationResource.UserRoles),
        new("Update UserRoles", ApplicationAction.Update, ApplicationResource.UserRoles),
        new("View Roles", ApplicationAction.View, ApplicationResource.Roles),
        new("Create Roles", ApplicationAction.Create, ApplicationResource.Roles),
        new("Update Roles", ApplicationAction.Update, ApplicationResource.Roles),
        new("Delete Roles", ApplicationAction.Delete, ApplicationResource.Roles),
        new("View RoleClaims", ApplicationAction.View, ApplicationResource.RoleClaims),
        new("Update RoleClaims", ApplicationAction.Update, ApplicationResource.RoleClaims),
        new("View Products", ApplicationAction.View, ApplicationResource.Products, IsBasic: true),
        new("Search Products", ApplicationAction.Search, ApplicationResource.Products, IsBasic: true),
        new("Create Products", ApplicationAction.Create, ApplicationResource.Products),
        new("Update Products", ApplicationAction.Update, ApplicationResource.Products),
        new("Delete Products", ApplicationAction.Delete, ApplicationResource.Products),
        new("Export Products", ApplicationAction.Export, ApplicationResource.Products),
        new("View Brands", ApplicationAction.View, ApplicationResource.Brands, IsBasic: true),
        new("Search Brands", ApplicationAction.Search, ApplicationResource.Brands, IsBasic: true),
        new("Create Brands", ApplicationAction.Create, ApplicationResource.Brands),
        new("Update Brands", ApplicationAction.Update, ApplicationResource.Brands),
        new("Delete Brands", ApplicationAction.Delete, ApplicationResource.Brands),
        new("Generate Brands", ApplicationAction.Generate, ApplicationResource.Brands),
        new("Clean Brands", ApplicationAction.Clean, ApplicationResource.Brands),
        new("View Tenants", ApplicationAction.View, ApplicationResource.Tenants, IsRoot: true),
        new("Create Tenants", ApplicationAction.Create, ApplicationResource.Tenants, IsRoot: true),
        new("Update Tenants", ApplicationAction.Update, ApplicationResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", ApplicationAction.UpgradeSubscription, ApplicationResource.Tenants, IsRoot: true)
    };

    public static IReadOnlyList<ApplicationPermission> All { get; } = new ReadOnlyCollection<ApplicationPermission>(_all);
    public static IReadOnlyList<ApplicationPermission> Root { get; } = new ReadOnlyCollection<ApplicationPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<ApplicationPermission> Admin { get; } = new ReadOnlyCollection<ApplicationPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<ApplicationPermission> Basic { get; } = new ReadOnlyCollection<ApplicationPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record ApplicationPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
