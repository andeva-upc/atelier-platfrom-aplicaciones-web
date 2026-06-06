using atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace atelier_platform_aplicaciones_web.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}
