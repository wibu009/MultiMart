using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Infrastructure.Identity.Role;
using MultiMart.Infrastructure.Identity.Token;
using MultiMart.Infrastructure.Identity.User;

namespace MultiMart.Infrastructure.Persistence.Configuration;

#region ApplicationUser
public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.UseTpcMappingStrategy();

        builder
            .Property(u => u.ObjectId)
            .HasMaxLength(256);

        builder
            .HasMany(u => u.Addresses)
            .WithOne()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", SchemaNames.Identity);
        builder.HasBaseType<ApplicationUser>();
        builder
            .Property(c => c.LoyaltyPoints)
            .HasDefaultValue(0);
    }
}

public class EmployeeConfig : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees", SchemaNames.Identity);
        builder.HasBaseType<ApplicationUser>();

        builder
            .Property(e => e.Position)
            .HasMaxLength(256);
        builder
            .Property(e => e.Department)
            .HasMaxLength(256);

        builder
            .HasOne(e => e.Manager)
            .WithMany()
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
#endregion

#region ApplicationRole
public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder) =>
        builder
            .ToTable("Roles", SchemaNames.Identity)
            .IsMultiTenant()
                .AdjustUniqueIndexes();
}
#endregion

#region ApplicationRoleClaim
public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
    {
        builder
            .ToTable("RoleClaims", SchemaNames.Identity)
            .IsMultiTenant();

        builder
            .HasOne(c => c.Role)
            .WithMany(r => r.RoleClaims)
            .HasForeignKey(c => c.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
#endregion

#region IdentityUserRole
public class IdentityUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder) =>
        builder
            .ToTable("UserRoles", SchemaNames.Identity)
            .IsMultiTenant();
}
#endregion

#region IdentityUserClaim
public class IdentityUserClaimConfig : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder) =>
        builder
            .ToTable("UserClaims", SchemaNames.Identity)
            .IsMultiTenant();
}
#endregion

#region IdentityUserLogin
public class IdentityUserLoginConfig : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder) =>
        builder
            .ToTable("UserLogins", SchemaNames.Identity)
            .IsMultiTenant();
}
#endregion

#region IdentityUserToken
public class IdentityUserTokenConfig : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder) =>
        builder
            .ToTable("UserTokens", SchemaNames.Identity)
            .IsMultiTenant();
}
#endregion

#region ApplicationUserRefreshToken
public class ApplicationUserRefreshTokenConfig : IEntityTypeConfiguration<ApplicationUserRefreshToken>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRefreshToken> builder)
    {
        builder
            .ToTable("UserRefreshTokens", SchemaNames.Identity)
            .IsMultiTenant();

        builder
            .Property(t => t.Token)
                .HasMaxLength(256);

        builder.HasOne(t => t.ApplicationUser)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
#endregion