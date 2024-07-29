using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Sales.Discounts;
using MultiMart.Infrastructure.Identity.User;

namespace MultiMart.Infrastructure.Persistence.Configuration.Sales;

public class DiscountConfig : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("Discounts", SchemaNames.Sales);

        builder.UseTptMappingStrategy();

        builder.Property(d => d.Name)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(d => d.StartDate)
            .IsRequired();
        builder.Property(d => d.EndDate)
            .IsRequired();

        builder.HasKey(d => d.Id);
    }
}

public class CustomerDiscountConfig : IEntityTypeConfiguration<CustomerDiscount>
{
    public void Configure(EntityTypeBuilder<CustomerDiscount> builder)
    {
        builder.ToTable("CustomerDiscounts", SchemaNames.Sales);

        builder.HasBaseType<Discount>();

        builder.Property(cd => cd.CustomerId)
            .IsRequired(false);

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(cd => cd.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProductDiscountConfig : IEntityTypeConfiguration<ProductDiscount>
{
    public void Configure(EntityTypeBuilder<ProductDiscount> builder)
    {
        builder.ToTable("ProductDiscounts", SchemaNames.Sales);

        builder.HasBaseType<Discount>();

        builder.Property(pd => pd.ProductId)
            .IsRequired(false);

        builder.HasOne(pd => pd.Product)
            .WithMany(p => p.Discounts)
            .HasForeignKey(pd => pd.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}