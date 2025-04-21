namespace Application.DTOs;

public record EnclosureDto(
    Guid Id,
    string Type,
    int Size,
    int Capacity,
    int CurrentCount
);
