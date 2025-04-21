using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Animal
{
    public AnimalId Id { get; }
    public string Species { get; private set; }
    public string Name { get; private set; }
    public DateTime BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public FoodType FavoriteFood { get; private set; }
    public HealthStatus Status { get; private set; }
    public EnclosureId? CurrentEnclosure { get; private set; }
    public EnclosureType PreferredEnclosureType { get; private set; }

    public Animal(
        AnimalId id,
        string species,
        string name,
        DateTime birthDate,
        Gender gender,
        FoodType favoriteFood,
        EnclosureType preferredEnclosureType)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Species = !string.IsNullOrWhiteSpace(species)
            ? species : throw new ArgumentException("Species required", nameof(species));
        Name = !string.IsNullOrWhiteSpace(name)
            ? name : throw new ArgumentException("Name required", nameof(name));
        BirthDate = birthDate;
        Gender = gender;
        FavoriteFood = favoriteFood ?? throw new ArgumentNullException(nameof(favoriteFood));
        PreferredEnclosureType = preferredEnclosureType;
        Status = HealthStatus.Healthy;
    }

    /// <summary>
    /// Кормит животное, поднимает FeedingTimeEvent.
    /// </summary>
    public void Feed()
    {
        if (Status != HealthStatus.Healthy)
            throw new InvalidOperationException("Невозможно кормить больное животное.");

        DomainEvents.Raise(new FeedingTimeEvent(Id, DateTimeOffset.UtcNow));
    }

    /// <summary>
    /// Лечит животное — возвращает в состояние Healthy.
    /// </summary>
    public void Treat()
    {
        Status = HealthStatus.Healthy;
    }

    /// <summary>
    /// Перемещает животное в другой вольер и поднимает AnimalMovedEvent.
    /// </summary>
    public void MoveTo(EnclosureId newEnclosure)
    {
        if (newEnclosure == null) throw new ArgumentNullException(nameof(newEnclosure));
        var oldEnclosure = CurrentEnclosure;
        CurrentEnclosure = newEnclosure;
        DomainEvents.Raise(new AnimalMovedEvent(Id, oldEnclosure, newEnclosure, DateTime.UtcNow));
    }

    /// <summary>
    /// Помечает животное больным.
    /// </summary>
    public void MarkSick()
    {
        Status = HealthStatus.Sick;
    }
    
    public void RemoveFromEnclosure()
    {
        CurrentEnclosure = null;
    }
}

public enum Gender
{
    Male,
    Female,
    Unknown
}

public enum HealthStatus
{
    Healthy,
    Sick
}