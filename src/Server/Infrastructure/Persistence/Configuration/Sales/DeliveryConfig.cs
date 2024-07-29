using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Sales.Deliveries;

namespace MultiMart.Infrastructure.Persistence.Configuration.Sales;

public class DeliveryConfig : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("Deliveries", SchemaNames.Sales);

        builder.Property(s => s.OrderId)
            .IsRequired(false);
        builder.Property(s => s.DeliveryRateId)
            .IsRequired(false);

        builder.HasKey(s => s.Id);
        builder.HasOne(s => s.Order)
            .WithOne(o => o.Delivery)
            .HasForeignKey<Delivery>(s => s.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.DeliveryRate)
            .WithMany(r => r.Deliveries)
            .HasForeignKey(s => s.DeliveryRateId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}