using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiMart.Domain.Catalog;
using MultiMart.Domain.Catalog.Addresses;
using MultiMart.Domain.Catalog.Characteristic.Book;
using MultiMart.Domain.Catalog.Deliveries;
using MultiMart.Domain.Catalog.Discounts;
using MultiMart.Domain.Catalog.Orders;
using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Catalog.Returns;
using MultiMart.Infrastructure.Identity.User;
using Newtonsoft.Json;

namespace MultiMart.Infrastructure.Persistence.Configuration;

#region Brand
public class BrandConfig : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands", SchemaNames.Catalog);

        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();
        builder.Property(b => b.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(b => b.Id);
    }
}
#endregion

#region Category
public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", SchemaNames.Catalog);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();
        builder.Property(c => c.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(c => c.Id);
    }
}
#endregion

#region Supplier
public class SupplierConfig : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers", SchemaNames.Catalog);

        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();
        builder.Property(s => s.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(s => s.Id);
    }
}
#endregion

#region Author
public class AuthorConfig : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors", SchemaNames.Catalog);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();
        builder.Property(a => a.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(a => a.Id);
    }
}
#endregion

#region Genre
public class GenreConfig : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres", SchemaNames.Catalog);

        builder.Property(g => g.Id)
            .ValueGeneratedOnAdd();
        builder.Property(g => g.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(g => g.Id);
        builder.HasMany(g => g.Books)
            .WithMany(b => b.Genres);
    }
}
#endregion

#region Series
public class SeriesConfig : IEntityTypeConfiguration<Series>
{
    public void Configure(EntityTypeBuilder<Series> builder)
    {
        builder.ToTable("Series", SchemaNames.Catalog);

        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();
        builder.Property(s => s.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasKey(s => s.Id);
    }
}
#endregion

#region Address
public class AddressConfig : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses", SchemaNames.Catalog);
        builder.UseTptMappingStrategy();

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.HasKey(a => a.Id);
        builder.HasQueryFilter(a => a.DeletedOn == null);
    }
}

public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.ToTable("UserAddresses", SchemaNames.Catalog);

        builder.HasBaseType<Address>();

        builder.Property(ua => ua.UserId);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class SupplierAddressConfig : IEntityTypeConfiguration<SupplierAddress>
{
    public void Configure(EntityTypeBuilder<SupplierAddress> builder)
    {
        builder.ToTable("SupplierAddresses", SchemaNames.Catalog);

        builder.HasBaseType<Address>();

        builder.Property(sa => sa.SupplierId);

        builder.HasOne(sa => sa.Supplier)
            .WithMany(s => s.Addresses)
            .HasForeignKey(sa => sa.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

#endregion

#region Product
public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.UseTptMappingStrategy();

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();
        builder.Property(p => p.Name)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(p => p.ImageUrls)
            .HasConversion(
                urls => JsonConvert.SerializeObject(urls),
                serializedUrls => JsonConvert.DeserializeObject<List<string>>(serializedUrls) ?? new List<string>());

        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books", SchemaNames.Catalog);
        builder.HasBaseType<Product>();

        builder.Property(b => b.Isbn)
            .HasMaxLength(256);
        builder.Property(b => b.Language)
            .HasMaxLength(256);
        builder.Property(b => b.Format)
            .HasMaxLength(256);
        builder.Property(b => b.Summary)
            .HasMaxLength(4000);

        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Series)
            .WithMany(s => s.Books)
            .HasForeignKey(b => b.SeriesId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(b => b.Genres)
            .WithMany(g => g.Books);
    }
}
#endregion

#region Review
public class ReviewConfig : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews", SchemaNames.Catalog);

        builder.Property(r => r.Title)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(r => r.Content)
            .HasMaxLength(4000);
        builder.Property(r => r.Rating)
            .IsRequired();

        builder.HasKey(r => r.Id);
        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
#endregion

#region Discount
public class DiscountConfig : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.UseTptMappingStrategy();

        builder.Property(d => d.Id)
            .ValueGeneratedOnAdd();
        builder.Property(d => d.Name)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(d => d.StartDate)
            .IsRequired();
        builder.Property(d => d.EndDate)
            .IsRequired();
    }
}

public class CustomerDiscountConfig : IEntityTypeConfiguration<CustomerDiscount>
{
    public void Configure(EntityTypeBuilder<CustomerDiscount> builder)
    {
        builder.ToTable("CustomerDiscounts", SchemaNames.Catalog);

        builder.HasBaseType<Discount>();

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(cd => cd.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProductDiscountConfig : IEntityTypeConfiguration<ProductDiscount>
{
    public void Configure(EntityTypeBuilder<ProductDiscount> builder)
    {
        builder.ToTable("ProductDiscounts", SchemaNames.Catalog);

        builder.HasBaseType<Discount>();

        builder.HasOne(pd => pd.Product)
            .WithMany(p => p.Discounts)
            .HasForeignKey(pd => pd.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
#endregion

#region Order
public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", SchemaNames.Catalog);

        builder.Property(o => o.Code)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(o => o.Note)
            .HasMaxLength(4000);

        builder.HasKey(o => o.Id);
        builder.HasOne(o => o.Return)
            .WithOne(r => r.Order)
            .HasForeignKey<Order>(o => o.ReturnId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(o => o.Delivery)
            .WithOne(s => s.Order)
            .HasForeignKey<Order>(o => o.DeliveryId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
#endregion

#region OrderItem
public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", SchemaNames.Catalog);

        builder.Property(oi => oi.Quantity)
            .IsRequired();
        builder.Property(oi => oi.BasePrice)
            .IsRequired();
        builder.Property(oi => oi.Discount)
            .IsRequired();

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
#endregion

#region OrderDiscount
public class OrderDiscountConfig : IEntityTypeConfiguration<OrderDiscount>
{
    public void Configure(EntityTypeBuilder<OrderDiscount> builder)
    {
        builder.ToTable("OrderDiscounts", SchemaNames.Catalog);

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
#endregion

#region Return
public class ReturnConfig : IEntityTypeConfiguration<Return>
{
    public void Configure(EntityTypeBuilder<Return> builder)
    {
        builder.ToTable("Returns", SchemaNames.Catalog);

        builder.HasKey(r => r.Id);
    }
}
#endregion

#region ReturnItem
public class ReturnItemConfig : IEntityTypeConfiguration<ReturnItem>
{
    public void Configure(EntityTypeBuilder<ReturnItem> builder)
    {
        builder.ToTable("ReturnItems", SchemaNames.Catalog);

        builder.Property(ri => ri.Quantity)
            .IsRequired();

        builder.HasKey(ri => ri.Id);
        builder.HasOne(ri => ri.Return)
            .WithMany(r => r.Items)
            .HasForeignKey(ri => ri.ReturnId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
#endregion

#region Delivery
public class DeliveryConfig : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("Deliveries", SchemaNames.Catalog);

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
#endregion

#region DeliveryRate
public class DeliveryRateConfig : IEntityTypeConfiguration<DeliveryRate>
{
    public void Configure(EntityTypeBuilder<DeliveryRate> builder)
    {
        builder.ToTable("DeliveryRates", SchemaNames.Catalog);

        builder.HasKey(sr => sr.Id);
        builder.HasOne(sr => sr.DeliveryCompany)
            .WithMany(sc => sc.DeliveryRates)
            .HasForeignKey(sr => sr.DeliveryCompanyId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
#endregion

#region DeliveryCompany
public class DeliveryCompanyConfig : IEntityTypeConfiguration<DeliveryCompany>
{
    public void Configure(EntityTypeBuilder<DeliveryCompany> builder)
    {
        builder.ToTable("DeliveryCompanies", SchemaNames.Catalog);

        builder.HasKey(sc => sc.Id);
    }
}
#endregion