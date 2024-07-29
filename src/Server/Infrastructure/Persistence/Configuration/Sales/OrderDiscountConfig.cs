using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Sales.Orders;

namespace MultiMart.Infrastructure.Persistence.Configuration.Sales;

public class OrderDiscountConfig : IEntityTypeConfiguration<OrderDiscount>
{
    public void Configure(EntityTypeBuilder<OrderDiscount> builder)
    {
        builder.ToTable("OrderDiscounts", SchemaNames.Sales);
        builder.Property(od => od.OrderId)
            .IsRequired(false);
        builder.Property(od => od.DiscountId)
            .IsRequired(false);

        builder.HasKey(od => od.Id);

        builder.HasOne(od => od.Order)
            .WithMany(o => o.Discounts)
            .HasForeignKey(od => od.OrderId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(od => od.Discount)
            .WithMany(d => d.Order)
            .HasForeignKey(od => od.DiscountId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}