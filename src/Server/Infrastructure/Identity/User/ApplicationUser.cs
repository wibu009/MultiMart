using Microsoft.AspNetCore.Identity;
using MultiMart.Domain.Catalog;
using MultiMart.Domain.Catalog.Order;
using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;
using MultiMart.Infrastructure.Identity.Token;

namespace MultiMart.Infrastructure.Identity.User;

public class ApplicationUser : IdentityUser, IAuditableEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? ObjectId { get; set; }
    public DefaultIdType CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DefaultIdType LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<ApplicationUserRefreshToken> RefreshTokens { get; set; } = new List<ApplicationUserRefreshToken>();
}

public class Customer : ApplicationUser
{
    public int LoyaltyPoints { get; set; }
    public List<Order> Orders { get; set; } = new();
}

public class Employee : ApplicationUser
{
    public string? Position { get; set; }
    public string? Department { get; set; }
    public DateTime? HireDate { get; set; }
    public string? ManagerId { get; set; }
    public virtual Employee Manager { get; set; } = default!;
}