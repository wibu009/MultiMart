using MultiMart.Application.Common.Interfaces;
using MultiMart.Shared.Events;

namespace MultiMart.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}