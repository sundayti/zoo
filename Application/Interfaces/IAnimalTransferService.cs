using Domain.ValueObjects;

namespace Application.Interfaces;
/// <summary>
/// Сервис для перемещения животных между вольерами.
/// </summary>
public interface IAnimalTransferService
{
    /// <summary>
    /// Переместить животное в указанный вольер.
    /// </summary>
    /// <param name="animalId">Идентификатор животного.</param>
    /// <param name="targetEnclosureId">Идентификатор целевого вольера.</param>
    Task TransferAsync(AnimalId animalId, EnclosureId targetEnclosureId);
}
