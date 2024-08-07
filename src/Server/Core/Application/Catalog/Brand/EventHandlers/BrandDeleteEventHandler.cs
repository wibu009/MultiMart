﻿using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Brand.EventHandlers;

public class BrandDeleteEventHandler : EventNotificationHandler<EntityDeletedEvent<Domain.Catalog.Brand>>
{
    private readonly ILogger<BrandDeleteEventHandler> _logger;

    public BrandDeleteEventHandler(ILogger<BrandDeleteEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityDeletedEvent<Domain.Catalog.Brand> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}