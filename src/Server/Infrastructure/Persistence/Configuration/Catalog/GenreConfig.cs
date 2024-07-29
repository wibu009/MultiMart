using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog.Characteristics.Book;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class GenreConfig : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres", SchemaNames.Catalog);

        builder.Property(g => g.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(g => g.Id);
    }
}