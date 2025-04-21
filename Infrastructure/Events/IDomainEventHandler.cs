using Domain.Events;

namespace Infrastructure.Events;

/// <summary>
/// Обработчик конкретного доменного события.
/// </summary>
public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent domainEvent);
}
