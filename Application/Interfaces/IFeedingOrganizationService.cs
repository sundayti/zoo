using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Interfaces;
/// <summary>
/// Сервис для управления расписанием и процессом кормления.
/// </summary>
public interface IFeedingOrganizationService
{
    /// <summary>
    /// Получить все кормления (опционально — за указанный день).
    /// </summary>
    /// <param name="date">Дата (UTC) для фильтрации; если null — вернуть все.</param>
    Task<IEnumerable<FeedingSchedule>> GetScheduleAsync(DateTimeOffset? date = null);

    /// <summary>
    /// Запланировать новое кормление.
    /// </summary>
    /// <param name="animalId">Идентификатор животного.</param>
    /// <param name="time">Время кормления (UTC).</param>
    /// <param name="foodType">Тип пищи.</param>
    Task ScheduleFeedingAsync(AnimalId animalId, DateTimeOffset time, FoodType foodType);

    /// <summary>
    /// Отметить кормление как выполненное.
    /// </summary>
    /// <param name="feedingScheduleId">Идентификатор записи в расписании.</param>
    Task MarkFeedingCompletedAsync(Guid feedingScheduleId);
}
