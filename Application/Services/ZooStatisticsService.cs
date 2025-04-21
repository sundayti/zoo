using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services;
/// <summary>
/// Сервис для сбора статистики по зоопарку.
/// </summary>
public class ZooStatisticsService : IStatisticsService
{
    private readonly IAnimalRepository _animalRepo;
    private readonly IEnclosureRepository _enclosureRepo;

    public ZooStatisticsService(
        IAnimalRepository animalRepo,
        IEnclosureRepository enclosureRepo)
    {
        _animalRepo = animalRepo;
        _enclosureRepo = enclosureRepo;
    }

    public async Task<ZooStatisticsDto> GetStatisticsAsync()
    {
        var animals = await _animalRepo.ListAsync();
        var enclosures = await _enclosureRepo.ListAsync();

        int totalAnimals = animals.Count;
        int totalEnclosures = enclosures.Count;
        int occupiedEnclosures = enclosures.Count(e => e.Animals.Any());
        int freeEnclosures = totalEnclosures - occupiedEnclosures;

        return new ZooStatisticsDto(
            TotalAnimals: totalAnimals,
            TotalEnclosures: totalEnclosures,
            OccupiedEnclosures: occupiedEnclosures,
            FreeEnclosures: freeEnclosures
        );
    }
}
