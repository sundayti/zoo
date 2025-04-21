using Domain.ValueObjects;

namespace Domain.Entities;
public class Enclosure
{
    public EnclosureId Id { get; }
    public EnclosureType Type { get; private set; }
    public int Size { get; private set; }
    public int Capacity { get; private set; }
    private readonly List<AnimalId> _animals = new();
    public IReadOnlyCollection<AnimalId> Animals => _animals.AsReadOnly();

    public Enclosure(EnclosureId id, EnclosureType type, int size, int capacity)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Type = type;
        if (size <= 0) throw new ArgumentException("Size must be positive", nameof(size));
        if (capacity <= 0) throw new ArgumentException("Capacity must be positive", nameof(capacity));
        Size = size;
        Capacity = capacity;
    }

    /// <summary>
    /// Добавляет животное в вольер, проверяя вместимость.
    /// </summary>
    public void AddAnimal(Animal animal)
    {
        if (animal == null) throw new ArgumentNullException(nameof(animal));
        if (_animals.Count >= Capacity)
            throw new InvalidOperationException("Вольер достиг максимальной вместимости.");

        if (!IsCompatible(animal))
            throw new InvalidOperationException(
                $"Невозможно поместить животное вида «{animal.PreferredEnclosureType.ToString()}» в вольер типа «{Type.ToString()}».");

        _animals.Add(animal.Id);
    }

    private bool IsCompatible(Animal animal)
    {
        return animal.PreferredEnclosureType == Type;
    }

    /// <summary>
    /// Убирает животное из вольера.
    /// </summary>
    public void RemoveAnimal(AnimalId animalId)
    {
        if (!_animals.Remove(animalId))
            throw new InvalidOperationException("Животное не найдено в этом вольере.");
    }
}
