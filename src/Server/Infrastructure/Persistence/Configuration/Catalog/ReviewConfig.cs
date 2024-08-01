using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog;
using MultiMart.Infrastructure.Identity.User;

namespace MultiMart.Infrastructure.Persistence.Configuration.Catalog;

public class ReviewConfig : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews", SchemaNames.Catalog);

        builder.Property(r => r.Title)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(r => r.Content)
            .HasMaxLength(4000);
        builder.Property(r => r.Rating)
            .IsRequired();
        builder.Property(r => r.CustomerId)
            .IsRequired(false);
        builder.Property(r => r.ProductId)
            .IsRequired(false);

        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}