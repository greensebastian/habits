using Habits.Core;

namespace Habits.Infrastructure;

public class HabitsRepository : IHabitsRepository
{
    public Task<UserProfile> Add(UserProfile userProfile, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserProfile?> GetUserProfileById(string userProfileId, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
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