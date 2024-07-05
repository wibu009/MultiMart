using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Brand.EventHandlers;

public class BrandUpdateEventHandler : EventNotificationHandler<EntityUpdatedEvent<Domain.Catalog.Brand>>
{
    private readonly ILogger<BrandUpdateEventHandler> _logger;

    public BrandUpdateEventHandler(ILogger<BrandUpdateEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityUpdatedEvent<Domain.Catalog.Brand> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}