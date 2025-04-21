namespace Application.DTOs;

/// <summary>
/// Статистика по зоопарку: общее число животных, число вольеров и их занятость.
/// </summary>
public record ZooStatisticsDto(
    int TotalAnimals,        
    int TotalEnclosures,    
    int OccupiedEnclosures,  
    int FreeEnclosures       
);
