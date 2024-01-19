namespace Habits.Core;

public readonly record struct Period(DateTimeOffset Start, DateTimeOffset End);

public class UserProfile
{
    public required string Id { get; init; }
    
    public required string Name { get; init; }
}

public record CreateUserProfileCommand(string Name);

public record GetUserProfileQuery(string UserProfileId);

public record PaginationQuery(int Offset = 0, int Count = 10);

public record PaginatedQuery(PaginationQuery Pagination);

public record Response<TData>(TData Data);

public record PaginationResponse(int Offset, int Count, int TotalCount);

public record PaginatedResponse<TData>(IList<TData> Data, PaginationResponse Pagination);

public class Direction
{
    public required string Id { get; init; }
    
    public required string Title { get; init; }
    
    public required string Motivation { get; init; }
    
    public required DateTimeOffset Start { get; init; }
    
    public required DateTimeOffset End { get; init; }
    
    public required string UserProfileId { get; init; }
}

public record CreateDirectionCommand(string UserProfileId, string Title, string Motivation, Period ActivePeriod);

public record GetDirectionsQuery(string UserProfileId, Period SearchPeriod, PaginationQuery Pagination) : PaginatedQuery(Pagination);

public class Habit
{
    public required string Id { get; init; }
    
    public required string Title { get; init; }
    
    public required string Frequency { get; init; }
    
    public DateTimeOffset Start { get; init; }
    
    public DateTimeOffset End { get; init; }
    
    public required string DirectionId { get; init; }
}

public record CreateHabitCommand(string DirectionId, string Title, string Frequency, Period ActivePeriod);

public record GetHabitsQuery(string UserProfileId, Period SearchPeriod, IEnumerable<string>? DirectionIds = null);

public class LogEntry
{
    public required string Id { get; init; }
    
    public required string HabitId { get; init; }
    
    public DateTimeOffset PerformedAt { get; init; }
    
    public string? Comment { get; }
}

public record CreateLogEntryCommand(string HabitId, DateTimeOffset PerformedAt, string? Comment = null);

public record GetLogEntriesQuery(string UserProfileId, Period SearchPeriod, IEnumerable<string>? DirectionIds = null, IEnumerable<string>? HabitIds = null);