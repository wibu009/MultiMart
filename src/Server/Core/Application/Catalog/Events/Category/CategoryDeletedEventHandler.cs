using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Events.Category;

public class CategoryDeletedEventHandler : EventNotificationHandler<EntityDeletedEvent<Domain.Catalog.Category>>
{
    private readonly ILogger<CategoryDeletedEventHandler> _logger;

    public CategoryDeletedEventHandler(ILogger<CategoryDeletedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityDeletedEvent<Domain.Catalog.Category> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}