using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Category.EventHandlers;

public class CategoryUpdateEventHandler : EventNotificationHandler<EntityUpdatedEvent<Domain.Catalog.Category>>
{
    private readonly ILogger<CategoryUpdateEventHandler> _logger;

    public CategoryUpdateEventHandler(ILogger<CategoryUpdateEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityUpdatedEvent<Domain.Catalog.Category> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}