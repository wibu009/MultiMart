using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Sales.Discounts;
using MultiMart.Infrastructure.Identity.User;

namespace MultiMart.Infrastructure.Persistence.Configuration.Sales;

public class DiscountConfig : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.UseTptMappingStrategy();

        builder.ToTable("Discounts", SchemaNames.Sales);

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

public class CustomerDiscountConfig : IEntityTypeConfiguration<DiscountOnCustomer>
{
    public void Configure(EntityTypeBuilder<DiscountOnCustomer> builder)
    {
        builder.ToTable("DiscountOnCustomers", SchemaNames.Sales);

        builder.Property(cd => cd.CustomerId)
            .IsRequired(false);
    }
}

public class ProductDiscountConfig : IEntityTypeConfiguration<DiscountOnProduct>
{
    public void Configure(EntityTypeBuilder<DiscountOnProduct> builder)
    {
        builder.ToTable("DiscountOnProducts", SchemaNames.Sales);

        builder.Property(pd => pd.ProductId)
            .IsRequired(false);

        builder.HasOne(pd => pd.Product)
            .WithMany(p => p.Discounts)
            .HasForeignKey(pd => pd.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}