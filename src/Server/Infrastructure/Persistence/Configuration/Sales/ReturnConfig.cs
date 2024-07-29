using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Sales.Returns;

namespace MultiMart.Infrastructure.Persistence.Configuration.Sales;

public class ReturnConfig : IEntityTypeConfiguration<Return>
{
    public void Configure(EntityTypeBuilder<Return> builder)
    {
        builder.ToTable("Returns", SchemaNames.Sales);

        builder.HasKey(r => r.Id);
    }
}