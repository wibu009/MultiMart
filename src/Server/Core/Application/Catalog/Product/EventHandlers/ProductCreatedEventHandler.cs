using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Product.EventHandlers;

public class ProductCreatedEventHandler : EventNotificationHandler<EntityCreatedEvent<Domain.Catalog.Product>>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<Domain.Catalog.Product> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}