using System.Collections.ObjectModel;

namespace MultiMart.Shared.Authorization;

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
    public const string AuditLogs = nameof(AuditLogs);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);
    public const string Categories = nameof(Categories);
    public const string Genres = nameof(Genres);
    public const string Authors = nameof(Authors);
}

public static class ApplicationPermissions
{
    private static readonly ApplicationPermission[] AllPermissions =
    {
        #region Dashboard
        new(ApplicationAction.View, ApplicationResource.Dashboard),
        #endregion

        #region Hangfire
        new(ApplicationAction.View, ApplicationResource.Hangfire),
        #endregion

        #region AuditLogs
        new(ApplicationAction.View, ApplicationResource.AuditLogs),
        new(ApplicationAction.Search, ApplicationResource.AuditLogs),
        #endregion

        #region Tenants
        new(ApplicationAction.View, ApplicationResource.Tenants, IsRoot: true),
        new(ApplicationAction.Create, ApplicationResource.Tenants, IsRoot: true),
        new(ApplicationAction.Update, ApplicationResource.Tenants, IsRoot: true),
        new(ApplicationAction.UpgradeSubscription, ApplicationResource.Tenants, IsRoot: true),
        #endregion

        #region Users
        new(ApplicationAction.View, ApplicationResource.Users),
        new(ApplicationAction.Search, ApplicationResource.Users),
        new(ApplicationAction.Create, ApplicationResource.Users),
        new(ApplicationAction.Update, ApplicationResource.Users),
        new(ApplicationAction.Delete, ApplicationResource.Users),
        new(ApplicationAction.Export, ApplicationResource.Users),
        #endregion

        #region UserRoles
        new(ApplicationAction.View, ApplicationResource.UserRoles),
        new(ApplicationAction.Update, ApplicationResource.UserRoles),
        #endregion

        #region Roles
        new(ApplicationAction.View, ApplicationResource.Roles),
        new(ApplicationAction.Create, ApplicationResource.Roles),
        new(ApplicationAction.Update, ApplicationResource.Roles),
        new(ApplicationAction.Delete, ApplicationResource.Roles),
        #endregion

        #region RoleClaims
        new(ApplicationAction.View, ApplicationResource.RoleClaims),
        new(ApplicationAction.Update, ApplicationResource.RoleClaims),
        #endregion

        #region Products
        new(ApplicationAction.View, ApplicationResource.Products, IsBasic: true),
        new(ApplicationAction.Search, ApplicationResource.Products, IsBasic: true),
        new(ApplicationAction.Create, ApplicationResource.Products),
        new(ApplicationAction.Update, ApplicationResource.Products),
        new(ApplicationAction.Delete, ApplicationResource.Products),
        new(ApplicationAction.Export, ApplicationResource.Products),
        new (ApplicationAction.Generate, ApplicationResource.Products),
        new (ApplicationAction.Clean, ApplicationResource.Products),
        #endregion

        #region Brands
        new(ApplicationAction.View, ApplicationResource.Brands, IsBasic: true),
        new(ApplicationAction.Search, ApplicationResource.Brands, IsBasic: true),
        new(ApplicationAction.Create, ApplicationResource.Brands),
        new(ApplicationAction.Update, ApplicationResource.Brands),
        new(ApplicationAction.Delete, ApplicationResource.Brands),
        new(ApplicationAction.Generate, ApplicationResource.Brands),
        new(ApplicationAction.Clean, ApplicationResource.Brands),
        #endregion

        #region Categories
        new(ApplicationAction.View, ApplicationResource.Categories, IsBasic: true),
        new(ApplicationAction.Search, ApplicationResource.Categories, IsBasic: true),
        new(ApplicationAction.Create, ApplicationResource.Categories),
        new(ApplicationAction.Update, ApplicationResource.Categories),
        new(ApplicationAction.Delete, ApplicationResource.Categories),
        new(ApplicationAction.Generate, ApplicationResource.Categories),
        new(ApplicationAction.Clean, ApplicationResource.Categories),
        #endregion
    };

    public static IReadOnlyList<ApplicationPermission> All { get; } = new ReadOnlyCollection<ApplicationPermission>(AllPermissions);
    public static IReadOnlyList<ApplicationPermission> Root { get; } = new ReadOnlyCollection<ApplicationPermission>(AllPermissions.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<ApplicationPermission> Admin { get; } = new ReadOnlyCollection<ApplicationPermission>(AllPermissions.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<ApplicationPermission> Basic { get; } = new ReadOnlyCollection<ApplicationPermission>(AllPermissions.Where(p => p.IsBasic).ToArray());
}

public record ApplicationPermission(string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
