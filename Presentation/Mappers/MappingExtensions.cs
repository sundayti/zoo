using Application.DTOs;
using Domain.Entities;
using Domain.ValueObjects;

namespace Presentation.Mappers;

public static class MappingExtensions
{
    /// <summary>
    /// Преобразует CreateAnimalDto в доменную сущность Animal.
    /// </summary>
    public static Animal ToDomain(this CreateAnimalDto dto)
    {
        if (!Enum.TryParse<Gender>(dto.Gender, true, out var gender))
            throw new ArgumentException(
                $"Invalid Gender «{dto.Gender}». Allowed: {string.Join(", ", Enum.GetNames<Gender>())}",
                nameof(dto.Gender));
        
        if (!Enum.TryParse<EnclosureType>(dto.PreferredEnclosureType, true, out var preferredType))
            throw new ArgumentException(
                $"Invalid Enclosure Type «{dto.PreferredEnclosureType}». Allowed: {string.Join(", ", Enum.GetNames<EnclosureType>())}",
                nameof(dto.PreferredEnclosureType));
        
        return new Animal(
            AnimalId.New(),
            dto.Species ?? throw new ArgumentNullException(nameof(dto.Species)),
            dto.Name    ?? throw new ArgumentNullException(nameof(dto.Name)),
            dto.BirthDate,
            gender,
            FoodType.From(dto.FavoriteFood ?? throw new ArgumentNullException(nameof(dto.FavoriteFood))),
            preferredType
        );
    }

    /// <summary>
    /// Преобразует доменную сущность Animal в AnimalDto.
    /// </summary>
    public static AnimalDto ToDto(this Animal a) =>
        new(
            Id:                a.Id.Value,
            Species:           a.Species,
            Name:              a.Name,
            BirthDate:         a.BirthDate,
            Gender:            a.Gender.ToString(),
            FavoriteFood:      a.FavoriteFood.Value,
            Status:            a.Status.ToString(),
            CurrentEnclosureId: a.CurrentEnclosure?.Value
        );

    /// <summary>
    /// Преобразует доменную сущность Enclosure в EnclosureDto.
    /// </summary>
    public static EnclosureDto ToDto(this Enclosure e) =>
        new(
            Id:           e.Id.Value,
            Type:         e.Type.ToString(),  
            Size:         e.Size,
            Capacity:     e.Capacity,
            CurrentCount: e.Animals.Count
        );

    /// <summary>
    /// Преобразует CreateEnclosureDto в доменную сущность Enclosure.
    /// </summary>
    public static Enclosure ToDomain(this CreateEnclosureDto dto)
    {
        if (!Enum.TryParse<EnclosureType>(dto.Type, true, out var type))
            throw new ArgumentException(
                $"Invalid Enclosure Type «{dto.Type}». Allowed: {string.Join(", ", Enum.GetNames<EnclosureType>())}",
                nameof(dto.Type));
        return new Enclosure(
            EnclosureId.New(),
            type,
            dto.Size,
            dto.Capacity
        );
    }

    /// <summary>
    /// Преобразует доменную сущность FeedingSchedule в FeedingScheduleDto.
    /// </summary>
    public static FeedingScheduleDto ToDto(this FeedingSchedule fs) =>
        new(
            Id:         fs.Id,
            AnimalId:   fs.AnimalId.Value,
            Time:       fs.Time,
            FoodType:   fs.FoodType.Value,
            Completed:  fs.Completed
        );
}