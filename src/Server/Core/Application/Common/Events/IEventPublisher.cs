using BookStack.Shared.Events;

namespace BookStack.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}