using Habits.Core;
using Microsoft.EntityFrameworkCore;

namespace Habits.Infrastructure;

public class HabitsRepository : IHabitsRepository
{
    private readonly HabitsContext _context;
    
    private const int MaxCount = 100;

    public HabitsRepository(HabitsContext context)
    {
        _context = context;
    }
    
    public async Task<UserProfile> Add(UserProfile userProfile, CancellationToken? cancellationToken = default)
    {
        await _context.AddAsync(userProfile);
        await _context.SaveChangesAsync();
        return userProfile;
    }

    public Task<UserProfile?> GetUserProfileById(string userProfileId, CancellationToken? cancellationToken = default)
    {
        var userProfile = _context.UserProfiles.SingleOrDefaultAsync(p => p.Id == userProfileId);
        return userProfile;
    }

    public async Task<Direction> Add(Direction direction, CancellationToken? cancellationToken = default)
    {
        await _context.AddAsync(direction);
        await _context.SaveChangesAsync();
        return direction;
    }

    public async Task<PaginatedResponse<Direction>> GetDirectionsByProfileAndPeriod(string userProfileId, Period searchPeriod, PaginationQuery pagination, CancellationToken? cancellationToken = default)
    {
        var toTake = Math.Min(pagination.Count, MaxCount);

        var directions = _context.Directions
            .Where(d => d.UserProfileId == userProfileId)
            .Where(d => searchPeriod.Start <= d.End && d.Start <= searchPeriod.End)
            .OrderBy(d => d.End);

        var totalCount = await directions.CountAsync();
        var paginatedDirections = await directions.Skip(pagination.Offset).Take(toTake).ToListAsync();

        return new PaginatedResponse<Direction>(paginatedDirections, new PaginationResponse(pagination.Offset, paginatedDirections.Count, totalCount));
    }

    public async Task<Habit> Add(Habit habit, CancellationToken? cancellationToken = default)
    {
        await _context.AddAsync(habit);
        await _context.SaveChangesAsync();
        return habit;
    }

    public async Task<PaginatedResponse<Habit>> GetHabitsByDirectionAndPeriod(string directionId, Period searchPeriod, PaginationQuery pagination,
        CancellationToken? cancellationToken = default)
    {
        var toTake = Math.Min(pagination.Count, MaxCount);

        var directions = _context.Habits
            .Where(d => d.DirectionId == directionId)
            .Where(d => searchPeriod.Start <= d.End && d.Start <= searchPeriod.End)
            .OrderBy(d => d.End);

        var totalCount = await directions.CountAsync();
        var paginatedHabits = await directions.Skip(pagination.Offset).Take(toTake).ToListAsync();

        return new PaginatedResponse<Habit>(paginatedHabits, new PaginationResponse(pagination.Offset, paginatedHabits.Count, totalCount));
    }
    
    public async Task<LogEntry> Add(LogEntry logEntry, CancellationToken? cancellationToken = default)
    {
        await _context.AddAsync(logEntry);
        await _context.SaveChangesAsync();
        return logEntry;
    }

    public async Task<PaginatedResponse<LogEntry>> GetLogEntriesByHabitAndPeriod(string habitId, Period searchPeriod, PaginationQuery pagination,
        CancellationToken? cancellationToken = default)
    {
        var toTake = Math.Min(pagination.Count, MaxCount);

        var directions = _context.LogEntries
            .Where(d => d.HabitId == habitId)
            .Where(d => searchPeriod.Start <= d.PerformedAt && d.PerformedAt <= searchPeriod.End)
            .OrderBy(d => d.PerformedAt);

        var totalCount = await directions.CountAsync();
        var paginatedLogEntries = await directions.Skip(pagination.Offset).Take(toTake).ToListAsync();

        return new PaginatedResponse<LogEntry>(paginatedLogEntries, new PaginationResponse(pagination.Offset, paginatedLogEntries.Count, totalCount));
    }
}