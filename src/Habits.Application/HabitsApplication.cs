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

    public async Task<Result<UserProfile>> CreateUserProfile(CreateUserProfileCommand command, CancellationToken? cancellationToken)
    {
        var userProfile = new UserProfile
        {
            Id = Guid.NewGuid().ToString("D"),
            Name = command.Name
        };
        return await _repository.Add(userProfile, cancellationToken);
    }

    public async Task<Result<UserProfile>> GetUserProfile(GetUserProfileQuery query, CancellationToken? cancellationToken)
    {
        var userProfile = await _repository.GetUserProfileById(query.UserProfileId, cancellationToken);

        return userProfile ?? new Result<UserProfile>(new NotFoundException(typeof(UserProfile), query.UserProfileId));
    }

    public async Task<Result<Direction>> CreateDirection(CreateDirectionCommand command, CancellationToken? cancellationToken)
    {
        return await _repository.Add(new Direction
        {
            Id = Guid.NewGuid().ToString("D"),
            Title = command.Title,
            Motivation = command.Motivation,
            Start = command.ActivePeriod.Start,
            End = command.ActivePeriod.End
        }, cancellationToken);
    }

    public async Task<Result<PaginatedResponse<Direction>>> GetDirections(GetDirectionsQuery query, CancellationToken? cancellationToken)
    {
        var directions =
            await _repository.GetDirectionsByProfileAndPeriod(query.UserProfileId, query.SearchPeriod,
                cancellationToken);

        return directions;
    }

    public Task<Result<Habit>> CreateHabit(CreateHabitCommand command, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<PaginatedResponse<Habit>>> GetHabits(GetHabitsQuery query, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<LogEntry>> CreateLogEntry(CreateLogEntryCommand command, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<PaginatedResponse<LogEntry>>> GetLogEntries(GetLogEntriesQuery query, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }
}