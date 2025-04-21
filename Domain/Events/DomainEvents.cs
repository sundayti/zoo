namespace Domain.Events;

/// <summary>
/// Простой диспетчер доменных событий.
/// Регистрацию обработчиков можно делать в Infrastructure (например, подписка на логирование, шину сообщений и т.п.).
/// </summary>
public static class DomainEvents
{
    // Храним список обработчиков
    private static readonly List<Action<IDomainEvent>> _handlers = new();

    /// <summary>
    /// Зарегистрировать новый обработчик для всех доменных событий.
    /// </summary>
    public static void RegisterHandler(Action<IDomainEvent> handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));
        _handlers.Add(handler);
    }

    /// <summary>
    /// Поднять доменное событие — он будет передан всем зарегистрированным обработчикам.
    /// </summary>
    public static void Raise(IDomainEvent domainEvent)
    {
        if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));
        foreach (var handler in _handlers)
        {
            handler(domainEvent);
        }
    }
}
