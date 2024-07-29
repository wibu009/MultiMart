using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog.Addresses;
using MultiMart.Infrastructure.Identity.User;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class AddressConfig : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses", SchemaNames.Catalog);
        builder.UseTptMappingStrategy();

        builder.HasKey(a => a.Id);
    }
}

public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.ToTable("UserAddresses", SchemaNames.Catalog);

        builder.HasBaseType<Address>();

        builder.Property(ua => ua.UserId)
            .IsRequired(false);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class SupplierAddressConfig : IEntityTypeConfiguration<SupplierAddress>
{
    public void Configure(EntityTypeBuilder<SupplierAddress> builder)
    {
        builder.ToTable("SupplierAddresses", SchemaNames.Catalog);

        builder.HasBaseType<Address>();

        builder.Property(sa => sa.SupplierId)
            .IsRequired(false);

        builder.HasOne(sa => sa.Supplier)
            .WithMany(s => s.Addresses)
            .HasForeignKey(sa => sa.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}