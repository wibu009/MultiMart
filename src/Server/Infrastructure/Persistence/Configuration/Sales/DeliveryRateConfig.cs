using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Sales.Deliveries;

namespace MultiMart.Infrastructure.Persistence.Configuration.Sales;

public class DeliveryRateConfig : IEntityTypeConfiguration<DeliveryRate>
{
    public void Configure(EntityTypeBuilder<DeliveryRate> builder)
    {
        builder.ToTable("DeliveryRates", SchemaNames.Sales);

        builder.Property(dr => dr.DeliveryCompanyId)
            .IsRequired(false);

        builder.HasKey(sr => sr.Id);
        builder.HasOne(sr => sr.DeliveryCompany)
            .WithMany(sc => sc.DeliveryRates)
            .HasForeignKey(sr => sr.DeliveryCompanyId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}