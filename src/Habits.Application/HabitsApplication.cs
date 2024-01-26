using DotNext;
using Habits.Core;

namespace Habits.Application;

public class HabitsApplication : IHabitsApplication
{
    private readonly IHabitsRepository _repository;

    public HabitsApplication(IHabitsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UserProfile>> CreateUserProfile(CreateUserProfileCommand command, CancellationToken cancellationToken = default)
    {
        return await _repository.Add(new UserProfile
        {
            Id = Guid.NewGuid().ToString("D"),
            Name = command.Name
        }, cancellationToken);
    }

    public async Task<Result<UserProfile>> GetUserProfile(GetUserProfileQuery query, CancellationToken cancellationToken = default)
    {
        var userProfile = await _repository.GetUserProfileById(query.UserProfileId, cancellationToken);

        return userProfile ?? new Result<UserProfile>(new NotFoundException(typeof(UserProfile), query.UserProfileId));
    }

    public async Task<Result<IList<UserProfile>>> GetAllUserProfiles(CancellationToken cancellationToken = default)
    {
        var userProfiles = await _repository.GetAllUserProfiles(cancellationToken);

        return Result.FromValue(userProfiles);
    }

    public async Task<Result<int>> DeleteUserProfile(DeleteUserProfileCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _repository.DeleteUserProfileById(command.UserProfileId, cancellationToken);

        return result;
    }

    public async Task<Result<Direction>> CreateDirection(CreateDirectionCommand command, CancellationToken cancellationToken = default)
    {
        return await _repository.Add(new Direction
        {
            Id = Guid.NewGuid().ToString("D"),
            Title = command.Title,
            Motivation = command.Motivation,
            Start = command.ActivePeriod.Start,
            End = command.ActivePeriod.End,
            UserProfileId = command.UserProfileId
        }, cancellationToken);
    }

    public async Task<Result<PaginatedResponse<Direction>>> GetDirections(GetDirectionsQuery query, CancellationToken cancellationToken = default)
    {
        var directions =
            await _repository.GetDirectionsByProfileAndPeriod(query.UserProfileId, query.SearchPeriod, query.Pagination, cancellationToken);

        return directions;
    }

    public async Task<Result<Habit>> CreateHabit(CreateHabitCommand command, CancellationToken cancellationToken = default)
    {
        return await _repository.Add(new Habit
        {
            Id = Guid.NewGuid().ToString("D"),
            Title = command.Title,
            Frequency = command.Frequency,
            DirectionId = command.DirectionId,
            Start = command.ActivePeriod.Start,
            End = command.ActivePeriod.End
        }, cancellationToken);
    }

    public async Task<Result<PaginatedResponse<Habit>>> GetHabits(GetHabitsQuery query, CancellationToken cancellationToken = default)
    {
        var habits = await _repository.GetHabitsByDirectionAndPeriod(query.DirectionId, query.SearchPeriod,
            query.Pagination, cancellationToken);

        return habits;
    }

    public async Task<Result<LogEntry>> CreateLogEntry(CreateLogEntryCommand command, CancellationToken cancellationToken = default)
    {
        return await _repository.Add(new LogEntry
        {
            Id = Guid.NewGuid().ToString("D"),
            Comment = command.Comment,
            HabitId = command.HabitId,
            PerformedAt = command.PerformedAt
        });
    }

    public async Task<Result<PaginatedResponse<LogEntry>>> GetLogEntries(GetLogEntriesQuery query, CancellationToken cancellationToken = default)
    {
        var logEntries = await _repository.GetLogEntriesByHabitAndPeriod(query.HabitId, query.SearchPeriod,
            query.Pagination, cancellationToken);

        return logEntries;
    }
}