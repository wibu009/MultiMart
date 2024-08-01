using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog.Addresses;
using MultiMart.Infrastructure.Identity.User;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class AddressConfig : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.UseTptMappingStrategy();

        builder.ToTable("Addresses", SchemaNames.Catalog);

        builder.HasKey(a => a.Id);
    }
}

public class AddressOfUserConfig : IEntityTypeConfiguration<AddressOfUser>
{
    public void Configure(EntityTypeBuilder<AddressOfUser> builder)
    {
        builder.ToTable("AddressOfUsers", SchemaNames.Catalog);
        builder.Property(ua => ua.UserId)
            .IsRequired(false);
    }
}

public class AddressOfSupplierConfig : IEntityTypeConfiguration<AddressOfSupplier>
{
    public void Configure(EntityTypeBuilder<AddressOfSupplier> builder)
    {
        builder.ToTable("AddressOfSuppliers", SchemaNames.Catalog);

        builder.Property(sa => sa.SupplierId)
            .IsRequired(false);

        builder.HasOne(sa => sa.Supplier)
            .WithMany(s => s.Addresses)
            .HasForeignKey(sa => sa.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}