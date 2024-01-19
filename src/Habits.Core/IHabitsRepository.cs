namespace Habits.Core;

public interface IHabitsRepository
{
    public Task<UserProfile> Add(UserProfile userProfile, CancellationToken? cancellationToken = default);

    public Task<UserProfile?> GetUserProfileById(string userProfileId, CancellationToken? cancellationToken = default);

    public Task<Direction> Add(Direction direction, CancellationToken? cancellationToken = default);

    public Task<PaginatedResponse<Direction>> GetDirectionsByProfileAndPeriod(string userProfileId, Period searchPeriod, PaginationQuery pagination, CancellationToken? cancellationToken = default);
    
    public Task<Habit> Add(Habit habit, CancellationToken? cancellationToken = default);
    
    public Task<PaginatedResponse<Habit>> GetHabitsByDirectionAndPeriod(string directionId, Period searchPeriod, PaginationQuery pagination, CancellationToken? cancellationToken = default);
}