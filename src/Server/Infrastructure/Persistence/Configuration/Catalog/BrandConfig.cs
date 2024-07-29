using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class BrandConfig : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands", SchemaNames.Catalog);

        builder.Property(b => b.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(b => b.Id);
    }
}