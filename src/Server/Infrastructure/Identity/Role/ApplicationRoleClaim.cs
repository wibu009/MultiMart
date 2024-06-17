using Microsoft.AspNetCore.Identity;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Infrastructure.Identity.Role;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IAuditableEntity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public virtual ApplicationRole Role { get; set; }
}