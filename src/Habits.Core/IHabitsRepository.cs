namespace Habits.Core;

public interface IHabitsRepository
{
    public Task<UserProfile> Add(UserProfile userProfile, CancellationToken? cancellationToken);

    public Task<UserProfile?> GetUserProfileById(string userProfileId, CancellationToken? cancellationToken);

    public Task<Direction> Add(Direction direction, CancellationToken? cancellationToken);

    public Task<PaginatedResponse<Direction>> GetDirectionsByProfileAndPeriod(string userProfileId, Period searchPeriod, CancellationToken? cancellationToken);
}