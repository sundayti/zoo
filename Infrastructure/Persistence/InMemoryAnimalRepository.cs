using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Infrastructure.Persistence;

public class InMemoryAnimalRepository : IAnimalRepository
{
    private readonly Dictionary<AnimalId, Animal> _store = new();

    public Task AddAsync(Animal animal)
    {
        if (_store.ContainsKey(animal.Id))
            throw new InvalidOperationException("Animal already exists");
        _store[animal.Id] = animal;
        return Task.CompletedTask;
    }

    public Task<Animal> GetByIdAsync(AnimalId id)
    {
        if (!_store.TryGetValue(id, out var animal))
            throw new KeyNotFoundException($"Animal {id} not found");
        return Task.FromResult(animal);
    }

    public Task UpdateAsync(Animal animal)
    {
        if (!_store.ContainsKey(animal.Id))
            throw new KeyNotFoundException($"Animal {animal.Id} not found");
        _store[animal.Id] = animal;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Animal animal)
    {
        if (!_store.Remove(animal.Id))
            throw new KeyNotFoundException($"Animal {animal.Id} not found");
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Animal>> ListAsync()
        => Task.FromResult((IReadOnlyList<Animal>)_store.Values.ToList());
}
