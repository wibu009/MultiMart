using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Sales.Orders;
using MultiMart.Infrastructure.Identity.User;

namespace MultiMart.Infrastructure.Persistence.Configuration.Sales;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", SchemaNames.Sales);

        builder.Property(o => o.Code)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(o => o.Note)
            .HasMaxLength(4000);
        builder.Property(o => o.ReturnId)
            .IsRequired(false);
        builder.Property(o => o.CustomerId)
            .IsRequired(false);

        builder.HasKey(o => o.Id);
        builder.HasOne(o => o.Return)
            .WithOne(r => r.Order)
            .HasForeignKey<Order>(o => o.ReturnId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}