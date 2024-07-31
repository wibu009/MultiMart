using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Events.Brand;

public class BrandCreatedEventHandler : EventNotificationHandler<EntityCreatedEvent<Domain.Catalog.Brand>>
{
    private readonly ILogger<BrandCreatedEventHandler> _logger;

    public BrandCreatedEventHandler(ILogger<BrandCreatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<Domain.Catalog.Brand> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}