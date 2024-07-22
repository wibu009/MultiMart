using MultiMart.Application.Common.Events;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Domain.Catalog;
using MultiMart.Domain.Common.Events;
using MultiMart.Domain.Identity;
using MultiMart.Shared.Events;
using MultiMart.Shared.Notifications;

namespace MultiMart.Application.Dashboard;

public class SendStatsChangedNotificationHandler :
    IEventNotificationHandler<ApplicationRoleCreatedEvent>,
    IEventNotificationHandler<ApplicationRoleDeletedEvent>,
    IEventNotificationHandler<ApplicationUserCreatedEvent>
{
    private readonly ILogger<SendStatsChangedNotificationHandler> _logger;
    private readonly INotificationSender _notifications;

    public SendStatsChangedNotificationHandler(ILogger<SendStatsChangedNotificationHandler> logger, INotificationSender notifications) =>
        (_logger, _notifications) = (logger, notifications);
    public Task Handle(EventNotification<ApplicationRoleCreatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<ApplicationRoleDeletedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);
    public Task Handle(EventNotification<ApplicationUserCreatedEvent> notification, CancellationToken cancellationToken) =>
        SendStatsChangedNotification(notification.Event, cancellationToken);

    private Task SendStatsChangedNotification(IEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Event} Triggered => Sending StatsChangedNotification", @event.GetType().Name);

        return _notifications.SendToAllAsync(new StatsChangedNotification(), cancellationToken);
    }
}