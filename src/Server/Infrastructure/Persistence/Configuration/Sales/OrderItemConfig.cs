using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Sales.Orders;

namespace MultiMart.Infrastructure.Persistence.Configuration.Sales;

public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", SchemaNames.Sales);

        builder.Property(oi => oi.Quantity)
            .IsRequired();
        builder.Property(oi => oi.BasePrice)
            .IsRequired();
        builder.Property(oi => oi.OrderId)
            .IsRequired(false);
        builder.Property(oi => oi.ProductId)
            .IsRequired(false);

        builder.HasKey(oi => oi.Id);
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}