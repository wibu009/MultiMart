using MultiMart.Infrastructure.Multitenancy;

namespace MultiMart.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(ApplicationTenantInfo tenant, CancellationToken cancellationToken);
}