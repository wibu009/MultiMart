using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog.Characteristics.Book;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class AuthorConfig : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors", SchemaNames.Catalog);

        builder.Property(a => a.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(a => a.Id);
    }
}