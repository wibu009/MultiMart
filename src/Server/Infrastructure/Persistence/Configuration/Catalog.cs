using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog;

namespace MultiMart.Infrastructure.Persistence.Configuration;

#region Brand
public class BrandConfig : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Name)
                .HasMaxLength(256);
    }
}
#endregion

#region Product
public class ProductTypeConfig : IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(p => p.Name)
            .HasMaxLength(1024);

        builder.HasMany(pt => pt.Products)
            .WithOne(p => p.ProductType)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.ProductDynamicProperties)
            .WithOne(p => p.ProductType)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Name)
                .HasMaxLength(1024);

        builder
            .Property(p => p.ImagePath)
                .HasMaxLength(2048);

        builder
            .HasMany(p => p.DynamicProperties)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class ProductDynamicPropertyConfig : IEntityTypeConfiguration<ProductDynamicProperty>
{
    public void Configure(EntityTypeBuilder<ProductDynamicProperty> builder)
    {
        builder.IsMultiTenant();

        builder
            .HasMany(p => p.Values)
            .WithOne(p => p.DynamicProperty)
            .HasForeignKey(p => p.DynamicPropertyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class ProductDynamicPropertyValueConfig : IEntityTypeConfiguration<ProductDynamicPropertyValue>
{
    public void Configure(EntityTypeBuilder<ProductDynamicPropertyValue> builder)
    {
        builder.IsMultiTenant();
    }
}

#endregion
