using Habits.Core;
using Microsoft.EntityFrameworkCore;

namespace Habits.Infrastructure;

public class HabitsRepository : IHabitsRepository
{
    private readonly HabitsContext _context;

    public HabitsRepository(HabitsContext context)
    {
        _context = context;
    }
    
    public async Task<UserProfile> Add(UserProfile userProfile, CancellationToken? cancellationToken)
    {
        await _context.AddAsync(userProfile);
        await _context.SaveChangesAsync();
        return userProfile;
    }

    public Task<UserProfile?> GetUserProfileById(string userProfileId, CancellationToken? cancellationToken)
    {
        var userProfile = _context.UserProfiles.SingleOrDefaultAsync(p => p.Id == userProfileId);
        return userProfile;
    }

    public Task<Direction> Add(Direction direction, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResponse<Direction>> GetDirectionsByProfileAndPeriod(string userProfileId, Period searchPeriod, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }
}