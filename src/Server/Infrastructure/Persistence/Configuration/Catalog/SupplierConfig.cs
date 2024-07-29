using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class SupplierConfig : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers", SchemaNames.Catalog);

        builder.Property(s => s.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(s => s.Id);
    }
}