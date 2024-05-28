using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using MultiMart.Infrastructure.Persistence.Configuration;

namespace MultiMart.Infrastructure.Multitenancy;

public class TenantDbContext : EFCoreStoreDbContext<ApplicationTenantInfo>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationTenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
    }
}