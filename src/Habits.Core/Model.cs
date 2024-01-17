namespace Habits.Core;

public readonly record struct Period(DateTimeOffset Start, DateTimeOffset End);

public class UserProfile
{
    public string Id { get; init; }
    
    public string Name { get; init; }
}

public record CreateUserProfileCommand(string Name);

public record GetUserProfileQuery(string UserProfileId);

public record PaginationQuery(int Offset, int Count);

public record PaginatedQuery(PaginationQuery Pagination);

public record Response<TData>(TData Data);

public record PaginationResponse(int Offset, int Count, int TotalCount);

public record PaginatedResponse<TData>(IEnumerable<TData> Data, PaginationResponse Pagination);

public class Direction
{
    public string Id { get; init; }
    
    public string Title { get; init; }
    
    public string Motivation { get; init; }
    
    public DateTimeOffset Start { get; init; }
    
    public DateTimeOffset End { get; init; }
}

public record CreateDirectionCommand(string UserProfileId, string Title, string Motivation, Period ActivePeriod);

public record GetDirectionsQuery(string UserProfileId, Period SearchPeriod, PaginationQuery Pagination) : PaginatedQuery(Pagination);

public class Habit
{
    public string Id { get; }
    
    public string Title { get; }
    
    public string Frequency { get; }
    
    public DateTimeOffset Start { get; }
    
    public DateTimeOffset End { get; }
}

public record CreateHabitCommand(string DirectionId, string Title, string Frequency, Period ActivePeriod);

public record GetHabitsQuery(string UserProfileId, Period SearchPeriod, IEnumerable<string>? DirectionIds = null);

public class LogEntry
{
    public string Id { get; }
    
    public string HabitId { get; }
    
    public DateTimeOffset PerformedAt { get; }
    
    public string? Comment { get; }
}

public record CreateLogEntryCommand(string HabitId, DateTimeOffset PerformedAt, string? Comment = null);

public record GetLogEntriesQuery(string UserProfileId, Period SearchPeriod, IEnumerable<string>? DirectionIds = null, IEnumerable<string>? HabitIds = null);