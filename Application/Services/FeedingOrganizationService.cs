using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.Services;
/// <summary>
/// Сервис для управления расписанием и процессом кормления.
/// </summary>
public class FeedingOrganizationService : IFeedingOrganizationService
{
    private readonly IFeedingScheduleRepository _scheduleRepo;
    private readonly IAnimalRepository _animalRepo;

    public FeedingOrganizationService(
        IFeedingScheduleRepository scheduleRepo,
        IAnimalRepository animalRepo)
    {
        _scheduleRepo = scheduleRepo;
        _animalRepo = animalRepo;
    }

    public async Task<IEnumerable<FeedingSchedule>> GetScheduleAsync(DateTimeOffset? date = null)
    {
        var all = await _scheduleRepo.ListAsync();
        if (date.HasValue)
        {
            var targetDate = date.Value.Date;
            return all.Where(fs => fs.Time.Date == targetDate).ToList();
        }
        return all;
    }

    public async Task ScheduleFeedingAsync(AnimalId animalId, DateTimeOffset time, FoodType foodType)
    {
        await _animalRepo.GetByIdAsync(animalId);

        var schedule = new FeedingSchedule(
            id: Guid.NewGuid(),
            animalId: animalId,
            time: time,
            foodType: foodType);

        await _scheduleRepo.AddAsync(schedule);
    }

    public async Task MarkFeedingCompletedAsync(Guid feedingScheduleId)
    {
        var schedule = await _scheduleRepo.GetByIdAsync(feedingScheduleId);
        schedule.MarkCompleted();
        await _scheduleRepo.UpdateAsync(schedule);
    }
}
