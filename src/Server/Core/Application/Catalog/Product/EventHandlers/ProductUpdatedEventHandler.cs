using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Product.EventHandlers;

public class ProductUpdatedEventHandler : EventNotificationHandler<EntityUpdatedEvent<Domain.Catalog.Product>>
{
    private readonly ILogger<ProductUpdatedEventHandler> _logger;

    public ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityUpdatedEvent<Domain.Catalog.Product> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}