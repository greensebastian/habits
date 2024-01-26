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
    
    public async Task<UserProfile> Add(UserProfile userProfile, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(userProfile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return userProfile;
    }

    public Task<UserProfile?> GetUserProfileById(string userProfileId, CancellationToken cancellationToken = default)
    {
        var userProfile = _context.UserProfiles.SingleOrDefaultAsync(p => p.Id == userProfileId, cancellationToken);
        return userProfile;
    }

    public async Task<IList<UserProfile>> GetAllUserProfiles(CancellationToken cancellationToken = default)
    {
        var userProfiles = await _context.UserProfiles.ToListAsync(cancellationToken);
        return userProfiles;
    }

    public async Task<int> DeleteUserProfileById(string userProfileId, CancellationToken cancellationToken = default)
    {
        var deleted = await _context.UserProfiles.Where(p => p.Id == userProfileId).ExecuteDeleteAsync(cancellationToken);
        return deleted;
    }

    public async Task<Direction> Add(Direction direction, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(direction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return direction;
    }

    public async Task<PaginatedResponse<Direction>> GetDirectionsByProfileAndPeriod(string userProfileId, Period searchPeriod, PaginationQuery pagination, CancellationToken cancellationToken = default)
    {
        var toTake = Math.Min(pagination.Count, MaxCount);

        var directions = _context.Directions
            .Where(d => d.UserProfileId == userProfileId)
            .Where(d => searchPeriod.Start <= d.End && d.Start <= searchPeriod.End)
            .OrderBy(d => d.End);

        var totalCount = await directions.CountAsync(cancellationToken);
        var paginatedDirections = await directions.Skip(pagination.Offset).Take(toTake).ToListAsync(cancellationToken);

        return new PaginatedResponse<Direction>(paginatedDirections, new PaginationResponse(pagination.Offset, paginatedDirections.Count, totalCount));
    }

    public async Task<Habit> Add(Habit habit, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(habit, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return habit;
    }

    public async Task<PaginatedResponse<Habit>> GetHabitsByDirectionAndPeriod(string directionId, Period searchPeriod, PaginationQuery pagination,
        CancellationToken cancellationToken = default)
    {
        var toTake = Math.Min(pagination.Count, MaxCount);

        var directions = _context.Habits
            .Where(d => d.DirectionId == directionId)
            .Where(d => searchPeriod.Start <= d.End && d.Start <= searchPeriod.End)
            .OrderBy(d => d.End);

        var totalCount = await directions.CountAsync(cancellationToken);
        var paginatedHabits = await directions.Skip(pagination.Offset).Take(toTake).ToListAsync(cancellationToken);

        return new PaginatedResponse<Habit>(paginatedHabits, new PaginationResponse(pagination.Offset, paginatedHabits.Count, totalCount));
    }
    
    public async Task<LogEntry> Add(LogEntry logEntry, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(logEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return logEntry;
    }

    public async Task<PaginatedResponse<LogEntry>> GetLogEntriesByHabitAndPeriod(string habitId, Period searchPeriod, PaginationQuery pagination,
        CancellationToken cancellationToken = default)
    {
        var toTake = Math.Min(pagination.Count, MaxCount);

        var directions = _context.LogEntries
            .Where(d => d.HabitId == habitId)
            .Where(d => searchPeriod.Start <= d.PerformedAt && d.PerformedAt <= searchPeriod.End)
            .OrderBy(d => d.PerformedAt);

        var totalCount = await directions.CountAsync(cancellationToken);
        var paginatedLogEntries = await directions.Skip(pagination.Offset).Take(toTake).ToListAsync(cancellationToken);

        return new PaginatedResponse<LogEntry>(paginatedLogEntries, new PaginationResponse(pagination.Offset, paginatedLogEntries.Count, totalCount));
    }
}