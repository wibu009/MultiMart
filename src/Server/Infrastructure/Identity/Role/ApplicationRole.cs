using Microsoft.AspNetCore.Identity;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Infrastructure.Identity.Role;

public class ApplicationRole : IdentityRole, IAuditableEntity
{
    public string? Description { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = new List<ApplicationRoleClaim>();
}