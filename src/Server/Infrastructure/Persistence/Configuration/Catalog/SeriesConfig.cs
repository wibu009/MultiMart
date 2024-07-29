using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog.Characteristics.Book;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class SeriesConfig : IEntityTypeConfiguration<Series>
{
    public void Configure(EntityTypeBuilder<Series> builder)
    {
        builder.ToTable("Series", SchemaNames.Catalog);

        builder.Property(s => s.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(s => s.Id);
    }
}