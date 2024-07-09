using Microsoft.AspNetCore.Identity;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Infrastructure.Identity.Role;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IAuditableEntity
{
    public DefaultIdType CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DefaultIdType LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public virtual ApplicationRole Role { get; set; } = null!;
}