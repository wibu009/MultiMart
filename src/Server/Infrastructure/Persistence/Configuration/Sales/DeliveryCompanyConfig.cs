using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Sales.Deliveries;

namespace MultiMart.Infrastructure.Persistence.Configuration.Sales;

public class DeliveryCompanyConfig : IEntityTypeConfiguration<DeliveryCompany>
{
    public void Configure(EntityTypeBuilder<DeliveryCompany> builder)
    {
        builder.ToTable("DeliveryCompanies", SchemaNames.Sales);

        builder.HasKey(sc => sc.Id);
    }
}