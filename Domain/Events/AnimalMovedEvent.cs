using Domain.ValueObjects;

namespace Domain.Events;

/// <summary>
/// Событие, возникающее при перемещении животного из одного вольера в другой.
/// </summary>
public sealed class AnimalMovedEvent : IDomainEvent
{
    public AnimalId AnimalId { get; }
    public EnclosureId FromEnclosureId { get; }
    public EnclosureId ToEnclosureId { get; }
    public DateTime OccurredOn { get; }

    public AnimalMovedEvent(AnimalId animalId, EnclosureId fromEnclosureId, EnclosureId toEnclosureId, DateTime occurredOn)
    {
        AnimalId = animalId;
        FromEnclosureId = fromEnclosureId;
        ToEnclosureId = toEnclosureId;
        OccurredOn = occurredOn;
    }
}
