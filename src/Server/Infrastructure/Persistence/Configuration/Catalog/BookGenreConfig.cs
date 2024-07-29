using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog.Characteristics.Book;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class BookGenreConfig : IEntityTypeConfiguration<BookGenre>
{
    public void Configure(EntityTypeBuilder<BookGenre> builder)
    {
        builder.ToTable("BookGenres", SchemaNames.Catalog);

        builder.Property(bg => bg.BookId)
            .IsRequired(false);
        builder.Property(bg => bg.GenreId)
            .IsRequired(false);

        builder.HasKey(bg => bg.Id);
        builder.HasOne(bg => bg.Book)
            .WithMany(b => b.Genres)
            .HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(bg => bg.Genre)
            .WithMany(g => g.Books)
            .HasForeignKey(bg => bg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}