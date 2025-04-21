using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities;
public class FeedingSchedule
{
    public Guid Id { get; }
    public AnimalId AnimalId { get; private set; }
    public DateTimeOffset Time { get; private set; }
    public FoodType FoodType { get; private set; }
    public bool Completed { get; private set; }

    public FeedingSchedule(Guid id, AnimalId animalId, DateTimeOffset time, FoodType foodType)
    {
        Id = id != Guid.Empty ? id : throw new ArgumentException("Id cannot be empty", nameof(id));
        AnimalId = animalId ?? throw new ArgumentNullException(nameof(animalId));
        Time = time;
        FoodType = foodType ?? throw new ArgumentNullException(nameof(foodType));
        Completed = false;
    }

    /// <summary>
    /// Отмечает кормление как выполненное и поднимает FeedingTimeEvent.
    /// </summary>
    public void MarkCompleted()
    {
        if (Completed)
            throw new InvalidOperationException("Кормление уже выполнено.");
        Completed = true;
        DomainEvents.Raise(new FeedingTimeEvent(AnimalId, Time));
    }
}
