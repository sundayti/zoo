using Domain.ValueObjects;

namespace Domain.Events;

/// <summary>
/// Событие, возникающее в момент кормления (или по расписанию).
/// </summary>
public sealed class FeedingTimeEvent : IDomainEvent
{
    /// <summary>Идентификатор животного, которое кормят.</summary>
    public AnimalId AnimalId { get; }

    /// <summary>
    /// Время, на которое было запланировано кормление (или момент отметки выполнения).
    /// </summary>
    public DateTimeOffset ScheduledTime { get; }

    /// <summary>Когда событие было поднято (UTC).</summary>
    public DateTime OccurredOn { get; }

    public FeedingTimeEvent(AnimalId animalId, DateTimeOffset scheduledTime)
    {
        AnimalId = animalId;
        ScheduledTime = scheduledTime;
        OccurredOn = DateTime.UtcNow;
    }
}