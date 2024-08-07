using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MultiMart.Application.Common.Events;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Domain.Catalog;
using MultiMart.Infrastructure.Multitenancy;
using MultiMart.Infrastructure.Persistence.Configuration;

namespace MultiMart.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, events)
    {
    }

    #region Product
    public DbSet<ProductType> ProductTypes => Set<ProductType>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductDynamicProperty> ProductDynamicProperties => Set<ProductDynamicProperty>();
    public DbSet<ProductDynamicPropertyValue> ProductDynamicPropertyValues => Set<ProductDynamicPropertyValue>();

    #endregion
    public DbSet<Brand> Brands => Set<Brand>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}