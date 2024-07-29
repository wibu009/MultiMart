using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Infrastructure.Identity.User;

namespace MultiMart.Infrastructure.Persistence.Configuration.Identity;

public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users", SchemaNames.Identity);
        builder.UseTptMappingStrategy();

        builder
            .Property(u => u.ObjectId)
            .HasMaxLength(256);

        builder
            .HasMany(u => u.Addresses)
            .WithOne()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);
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
            .Property(e => e.ManagerId)
            .IsRequired(false);

        builder
            .HasOne(e => e.Manager)
            .WithMany()
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}