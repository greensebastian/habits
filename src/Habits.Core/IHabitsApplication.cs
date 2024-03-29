﻿using DotNext;

namespace Habits.Core;

public interface IHabitsApplication
{
    public Task<Result<UserProfile>> CreateUserProfile(CreateUserProfileCommand command, CancellationToken cancellationToken = default);

    public Task<Result<UserProfile>> GetUserProfile(GetUserProfileQuery query, CancellationToken cancellationToken = default);

    public Task<Result<IList<UserProfile>>> GetAllUserProfiles(CancellationToken cancellationToken = default);

    public Task<Result<int>> DeleteUserProfile(DeleteUserProfileCommand command, CancellationToken cancellationToken = default);
    
    public Task<Result<Direction>> CreateDirection(CreateDirectionCommand command, CancellationToken cancellationToken = default);

    public Task<Result<PaginatedResponse<Direction>>> GetDirections(GetDirectionsQuery query, CancellationToken cancellationToken = default);

    public Task<Result<Habit>> CreateHabit(CreateHabitCommand command, CancellationToken cancellationToken = default);

    public Task<Result<PaginatedResponse<Habit>>> GetHabits(GetHabitsQuery query, CancellationToken cancellationToken = default);

    public Task<Result<LogEntry>> CreateLogEntry(CreateLogEntryCommand command, CancellationToken cancellationToken = default);

    public Task<Result<PaginatedResponse<LogEntry>>> GetLogEntries(GetLogEntriesQuery query, CancellationToken cancellationToken = default);
}