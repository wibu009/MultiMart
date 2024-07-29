using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Sales.Returns;

namespace MultiMart.Infrastructure.Persistence.Configuration.Sales;

public class ReturnItemConfig : IEntityTypeConfiguration<ReturnItem>
{
    public void Configure(EntityTypeBuilder<ReturnItem> builder)
    {
        builder.ToTable("ReturnItems", SchemaNames.Sales);

        builder.Property(ri => ri.Quantity)
            .IsRequired();
        builder.Property(ri => ri.ReturnId)
            .IsRequired(false);

        builder.HasKey(ri => ri.Id);
        builder.HasOne(ri => ri.Return)
            .WithMany(r => r.Items)
            .HasForeignKey(ri => ri.ReturnId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}