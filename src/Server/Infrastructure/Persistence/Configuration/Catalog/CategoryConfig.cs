using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", SchemaNames.Catalog);

        builder.Property(c => c.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(c => c.Id);
    }
}