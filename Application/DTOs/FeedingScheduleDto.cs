// Application/DTOs/FeedingScheduleDto.cs
namespace Application.DTOs;

public record FeedingScheduleDto(
    Guid Id,
    Guid AnimalId,
    DateTimeOffset Time,
    string FoodType,
    bool Completed
);
