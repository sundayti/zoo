using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Репозиторий для работы с сущностями FeedingSchedule.
/// </summary>
public interface IFeedingScheduleRepository
{
    Task AddAsync(FeedingSchedule schedule);
    Task<FeedingSchedule> GetByIdAsync(Guid id);
    Task UpdateAsync(FeedingSchedule schedule);
    Task RemoveAsync(FeedingSchedule schedule);
    Task<IReadOnlyList<FeedingSchedule>> ListAsync();
}
