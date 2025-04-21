using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces;

/// <summary>
/// Репозиторий для работы с сущностями Enclosure.
/// </summary>
public interface IEnclosureRepository
{
    Task AddAsync(Enclosure enclosure);
    Task<Enclosure> GetByIdAsync(EnclosureId id);
    Task UpdateAsync(Enclosure enclosure);
    Task RemoveAsync(Enclosure enclosure);
    Task<IReadOnlyList<Enclosure>> ListAsync();
}