using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Events.Category;

public class CategoryCreatedEventHandler : EventNotificationHandler<EntityCreatedEvent<Domain.Catalog.Category>>
{
    private readonly ILogger<CategoryCreatedEventHandler> _logger;

    public CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<Domain.Catalog.Category> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}