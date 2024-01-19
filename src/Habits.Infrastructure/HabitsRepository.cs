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
            .OrderByDescending(d => d.End);

        var totalCount = await directions.CountAsync();
        var paginatedDirections = await directions.Skip(pagination.Offset).Take(toTake).ToListAsync();

        return new PaginatedResponse<Direction>(paginatedDirections, new PaginationResponse(pagination.Offset, paginatedDirections.Count, totalCount));
    }
}