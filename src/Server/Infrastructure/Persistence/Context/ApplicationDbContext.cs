using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MultiMart.Application.Common.Events;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Domain.Catalog;
using MultiMart.Domain.Catalog.Addresses;
using MultiMart.Domain.Catalog.Characteristics.Book;
using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Sales.Deliveries;
using MultiMart.Domain.Sales.Discounts;
using MultiMart.Domain.Sales.Orders;
using MultiMart.Domain.Sales.Returns;
using MultiMart.Infrastructure.Identity.Token;
using MultiMart.Infrastructure.Identity.User;
using MultiMart.Infrastructure.Persistence.Configuration;
using Address = MultiMart.Domain.Catalog.Addresses.Address;

namespace MultiMart.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, events)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }

    #region Identity
    public virtual DbSet<Customer> Customers { get; set; } = default!;
    public virtual DbSet<Employee> Employees { get; set; } = default!;
    public virtual DbSet<ApplicationUserRefreshToken> RefreshTokens { get; set; } = default!;
    #endregion

    #region Catalog
    public virtual DbSet<Address> Addresses { get; set; } = default!;
    public virtual DbSet<AddressOfUser> UserAddresses { get; set; } = default!;
    public virtual DbSet<AddressOfSupplier> SupplierAddresses { get; set; } = default!;
    public virtual DbSet<Brand> Brands { get; set; } = default!;
    public virtual DbSet<Category> Categories { get; set; } = default!;
    public virtual DbSet<Supplier> Suppliers { get; set; } = default!;
    public virtual DbSet<Genre> Genres { get; set; } = default!;
    public virtual DbSet<BookGenre> BookGenres { get; set; } = default!;
    public virtual DbSet<Author> Authors { get; set; } = default!;
    public virtual DbSet<Series> Series { get; set; } = default!;
    public virtual DbSet<Product> Products { get; set; } = default!;
    public virtual DbSet<Book> Books { get; set; } = default!;
    public virtual DbSet<Review> Reviews { get; set; } = default!;
    #endregion

    #region Sales
    public virtual DbSet<Order> Orders { get; set; } = default!;
    public virtual DbSet<OrderItem> OrderItems { get; set; } = default!;
    public virtual DbSet<Return> Returns { get; set; } = default!;
    public virtual DbSet<ReturnItem> ReturnItems { get; set; } = default!;
    public virtual DbSet<Discount> Discounts { get; set; } = default!;
    public virtual DbSet<DiscountOnCustomer> CustomerDiscounts { get; set; } = default!;
    public virtual DbSet<DiscountOnProduct> ProductDiscounts { get; set; } = default!;
    public virtual DbSet<Delivery> Deliveries { get; set; } = default!;
    public virtual DbSet<DeliveryCompany> DeliveryCompanies { get; set; } = default!;
    public virtual DbSet<DeliveryRate> DeliveryRates { get; set; } = default!;
    #endregion
}