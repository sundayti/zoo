using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces;

/// <summary>
/// Репозиторий для работы с сущностями Animal.
/// </summary>
public interface IAnimalRepository
{
    Task AddAsync(Animal animal);
    Task<Animal> GetByIdAsync(AnimalId id);
    Task UpdateAsync(Animal animal);
    Task RemoveAsync(Animal animal);
    Task<IReadOnlyList<Animal>> ListAsync();
}
