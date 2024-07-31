using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Events.Brand;

public class BrandUpdatedEventHandler : EventNotificationHandler<EntityUpdatedEvent<Domain.Catalog.Brand>>
{
    private readonly ILogger<BrandUpdatedEventHandler> _logger;

    public BrandUpdatedEventHandler(ILogger<BrandUpdatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityUpdatedEvent<Domain.Catalog.Brand> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}