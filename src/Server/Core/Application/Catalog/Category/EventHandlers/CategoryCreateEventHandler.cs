using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Category.EventHandlers;

public class CategoryCreateEventHandler : EventNotificationHandler<EntityCreatedEvent<Domain.Catalog.Category>>
{
    private readonly ILogger<CategoryCreateEventHandler> _logger;

    public CategoryCreateEventHandler(ILogger<CategoryCreateEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<Domain.Catalog.Category> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}