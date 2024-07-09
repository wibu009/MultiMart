using Microsoft.AspNetCore.Identity;
using MultiMart.Domain.Common.Contracts;
using MultiMart.Infrastructure.Identity.Token;

namespace MultiMart.Infrastructure.Identity.User;

public class ApplicationUser : IdentityUser, IAuditableEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? ObjectId { get; set; }
    public DefaultIdType CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DefaultIdType LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public ICollection<ApplicationUserRefreshToken> RefreshTokens { get; set; } = new List<ApplicationUserRefreshToken>();
}