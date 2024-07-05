using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Product.EventHandlers;

public class ProductDeletedEventHandler : EventNotificationHandler<EntityDeletedEvent<Domain.Catalog.Product>>
{
    private readonly ILogger<ProductDeletedEventHandler> _logger;

    public ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityDeletedEvent<Domain.Catalog.Product> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}