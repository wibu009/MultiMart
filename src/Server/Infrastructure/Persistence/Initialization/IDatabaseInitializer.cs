using BookStack.Infrastructure.Multitenancy;

namespace BookStack.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(ApplicationTenantInfo tenant, CancellationToken cancellationToken);
}