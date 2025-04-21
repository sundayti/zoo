namespace Domain.Events;

/// <summary>
/// Маркерный интерфейс для доменных событий.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Момент возникновения события в UTC.
    /// </summary>
    DateTime OccurredOn { get; }
}
