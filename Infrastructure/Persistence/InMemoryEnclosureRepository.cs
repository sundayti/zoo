using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Infrastructure.Persistence;

public class InMemoryEnclosureRepository : IEnclosureRepository
{
    private readonly Dictionary<EnclosureId, Enclosure> _store = new();

    public Task AddAsync(Enclosure enclosure)
    {
        if (_store.ContainsKey(enclosure.Id))
            throw new InvalidOperationException("Enclosure already exists");
        _store[enclosure.Id] = enclosure;
        return Task.CompletedTask;
    }

    public Task<Enclosure> GetByIdAsync(EnclosureId id)
    {
        if (!_store.TryGetValue(id, out var enc))
            throw new KeyNotFoundException($"Enclosure {id} not found");
        return Task.FromResult(enc);
    }

    public Task UpdateAsync(Enclosure enclosure)
    {
        if (!_store.ContainsKey(enclosure.Id))
            throw new KeyNotFoundException($"Enclosure {enclosure.Id} not found");
        _store[enclosure.Id] = enclosure;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Enclosure enclosure)
    {
        if (!_store.Remove(enclosure.Id))
            throw new KeyNotFoundException($"Enclosure {enclosure.Id} not found");
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Enclosure>> ListAsync()
        => Task.FromResult((IReadOnlyList<Enclosure>)_store.Values.ToList());
}
