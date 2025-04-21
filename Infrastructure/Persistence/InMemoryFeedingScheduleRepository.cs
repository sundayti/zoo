using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistence;
public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
{
    private readonly Dictionary<Guid, FeedingSchedule> _store = new();

    public Task AddAsync(FeedingSchedule schedule)
    {
        if (_store.ContainsKey(schedule.Id))
            throw new InvalidOperationException("Schedule already exists");
        _store[schedule.Id] = schedule;
        return Task.CompletedTask;
    }

    public Task<FeedingSchedule> GetByIdAsync(Guid id)
    {
        if (!_store.TryGetValue(id, out var sched))
            throw new KeyNotFoundException($"FeedingSchedule {id} not found");
        return Task.FromResult(sched);
    }

    public Task UpdateAsync(FeedingSchedule schedule)
    {
        if (!_store.ContainsKey(schedule.Id))
            throw new KeyNotFoundException($"FeedingSchedule {schedule.Id} not found");
        _store[schedule.Id] = schedule;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(FeedingSchedule schedule)
    {
        if (!_store.Remove(schedule.Id))
            throw new KeyNotFoundException($"FeedingSchedule {schedule.Id} not found");
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<FeedingSchedule>> ListAsync()
        => Task.FromResult((IReadOnlyList<FeedingSchedule>)_store.Values.ToList());
}