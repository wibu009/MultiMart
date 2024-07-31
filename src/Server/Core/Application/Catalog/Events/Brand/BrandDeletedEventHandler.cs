using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Events.Brand;

public class BrandDeletedEventHandler : EventNotificationHandler<EntityDeletedEvent<Domain.Catalog.Brand>>
{
    private readonly ILogger<BrandDeletedEventHandler> _logger;

    public BrandDeletedEventHandler(ILogger<BrandDeletedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityDeletedEvent<Domain.Catalog.Brand> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}