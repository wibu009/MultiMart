using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Brand.EventHandlers;

public class BrandCreateEventHandler : EventNotificationHandler<EntityCreatedEvent<Domain.Catalog.Brand>>
{
    private readonly ILogger<BrandCreateEventHandler> _logger;

    public BrandCreateEventHandler(ILogger<BrandCreateEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<Domain.Catalog.Brand> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}