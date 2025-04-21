using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Сервис для сбора и предоставления статистики по состоянию зоопарка.
/// </summary>
public interface IStatisticsService
{
    /// <summary>
    /// Получить текущую статистику (количество животных, свободные вольеры и т.д.).
    /// </summary>
    Task<ZooStatisticsDto> GetStatisticsAsync();
}
