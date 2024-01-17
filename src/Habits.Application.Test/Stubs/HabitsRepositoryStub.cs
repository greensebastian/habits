using Habits.Application;

namespace Habits.Core.Test.Stubs;

public class HabitsRepositoryStub : IHabitsRepository
{
    private List<UserProfile> UserProfiles { get; } = [];
    
    public Task<UserProfile> Add(UserProfile userProfile, CancellationToken? cancellationToken)
    {
        UserProfiles.Add(userProfile);
        return Task.FromResult(userProfile);
    }

    public Task<UserProfile?> GetUserProfileById(string userProfileId, CancellationToken? cancellationToken)
    {
        return Task.FromResult(UserProfiles.SingleOrDefault(prof => prof.Id == userProfileId) ?? null);
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