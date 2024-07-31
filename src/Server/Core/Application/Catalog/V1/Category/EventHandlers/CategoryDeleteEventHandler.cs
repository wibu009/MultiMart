using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Category.EventHandlers;

public class CategoryDeleteEventHandler : EventNotificationHandler<EntityDeletedEvent<Domain.Catalog.Category>>
{
    private readonly ILogger<CategoryDeleteEventHandler> _logger;

    public CategoryDeleteEventHandler(ILogger<CategoryDeleteEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityDeletedEvent<Domain.Catalog.Category> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}