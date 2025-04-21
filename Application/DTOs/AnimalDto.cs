namespace Application.DTOs;

public record AnimalDto(
    Guid Id,
    string Species,
    string Name,
    DateTime BirthDate,
    string Gender,
    string FavoriteFood,
    string Status,
    Guid? CurrentEnclosureId
);
