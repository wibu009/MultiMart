using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog.Products;
using Newtonsoft.Json;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.UseTptMappingStrategy();

        builder.Property(p => p.Name)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(p => p.ImageUrls)
            .HasConversion(
                urls => JsonConvert.SerializeObject(urls),
                serializedUrls => JsonConvert.DeserializeObject<List<string>>(serializedUrls) ?? new List<string>());
        builder.Property(p => p.BrandId)
            .IsRequired(false);
        builder.Property(p => p.CategoryId)
            .IsRequired(false);
        builder.Property(p => p.SupplierId)
            .IsRequired(false);

        builder.HasKey(p => p.Id);
        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books", SchemaNames.Catalog);
        builder.HasBaseType<Product>();

        builder.Property(b => b.Isbn)
            .HasMaxLength(256);
        builder.Property(b => b.Language)
            .HasMaxLength(256);
        builder.Property(b => b.Format)
            .HasMaxLength(256);
        builder.Property(b => b.Summary)
            .HasMaxLength(4000);
        builder.Property(b => b.SeriesId)
            .IsRequired(false);
        builder.Property(b => b.AuthorId)
            .IsRequired(false);

        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Series)
            .WithMany(s => s.Books)
            .HasForeignKey(b => b.SeriesId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}