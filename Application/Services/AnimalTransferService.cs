using Application.Interfaces;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.Services;
public class AnimalTransferService : IAnimalTransferService
{
    private readonly IAnimalRepository _animalRepo;
    private readonly IEnclosureRepository _enclosureRepo;

    public AnimalTransferService(
        IAnimalRepository animalRepo,
        IEnclosureRepository enclosureRepo)
    {
        _animalRepo = animalRepo;
        _enclosureRepo = enclosureRepo;
    }

    public async Task TransferAsync(AnimalId animalId, EnclosureId targetEnclosureId)
    {
        var animal = await _animalRepo.GetByIdAsync(animalId);
        var toEnclosure = await _enclosureRepo.GetByIdAsync(targetEnclosureId);
        if (toEnclosure.Animals.Count >= toEnclosure.Capacity)
            throw new InvalidOperationException($"Enclosure {targetEnclosureId.Value} is full.");
        if (animal.CurrentEnclosure is not null)
        {
            var fromEnclosure = await _enclosureRepo.GetByIdAsync(animal.CurrentEnclosure);
            fromEnclosure.RemoveAnimal(animalId);
            await _enclosureRepo.UpdateAsync(fromEnclosure);
        }

        toEnclosure.AddAnimal(animal);
        await _enclosureRepo.UpdateAsync(toEnclosure);

        animal.MoveTo(targetEnclosureId);
        await _animalRepo.UpdateAsync(animal);
    }
}