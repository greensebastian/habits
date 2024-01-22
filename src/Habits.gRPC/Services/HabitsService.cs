using Grpc.Core;
using Habits.Core;

namespace Habits.gRPC.Services;

public class HabitsService : gRPC.HabitsService.HabitsServiceBase
{
    private readonly IHabitsApplication _application;

    public HabitsService(IHabitsApplication application)
    {
        _application = application;
    }
    
    public override async Task<UserProfile> CreateProfile(CreateProfileRequest request, ServerCallContext context)
    {
        var command = new CreateUserProfileCommand(request.Name);
        var userProfile = await _application.CreateUserProfile(command);
        return new UserProfile
        {
            Id = userProfile.Value.Id,
            Name = userProfile.Value.Name
        };
    }

    public override async Task<UserProfile> GetUserProfile(GetUserProfileRequest request, ServerCallContext context)
    {
        var query = new GetUserProfileQuery(request.Id);
        var userProfile = await _application.GetUserProfile(query);
        return new UserProfile
        {
            Id = userProfile.Value.Id,
            Name = userProfile.Value.Name
        };
    }

    public override async Task<Direction> CreateDirection(CreateDirectionRequest request, ServerCallContext context)
    {
        var command = new CreateDirectionCommand(request.UserProfileId, request.Title, request.Motivation, CorePeriod(request.ActivePeriod));
        var direction = await _application.CreateDirection(command);
        return DtoDirection(direction.Value);
    }

    public override async Task<PaginatedDirections> GetDirections(GetDirectionsRequest request, ServerCallContext context)
    {
        var query = new GetDirectionsQuery(request.UserProfileId, CorePeriod(request.SearchPeriod),
            CorePaginationQuery(request.Pagination));
        var directions = await _application.GetDirections(query);
        return new PaginatedDirections
        {
            Data = { directions.Value.Data.Select(DtoDirection) },
            Pagination = DtoPagination(directions.Value.Pagination)
        };
    }

    public override async Task<Habit> CreateHabit(CreateHabitRequest request, ServerCallContext context)
    {
        var command = new CreateHabitCommand(request.DirectionId, request.Title, request.Frequency,
            CorePeriod(request.ActivePeriod));
        var habit = await _application.CreateHabit(command);
        return DtoHabit(habit.Value);
    }

    public override async Task<PaginatedHabits> GetHabits(GetHabitsRequest request, ServerCallContext context)
    {
        var query = new GetHabitsQuery(request.DirectionId, CorePeriod(request.SearchPeriod),
            CorePaginationQuery(request.Pagination));
        var habits = await _application.GetHabits(query);
        return new PaginatedHabits
        {
            Data = { habits.Value.Data.Select(DtoHabit) },
            Pagination = DtoPagination(habits.Value.Pagination)
        };
    }

    public override async Task<LogEntry> CreateLogEntry(CreateLogEntryRequest request, ServerCallContext context)
    {
        var command =
            new CreateLogEntryCommand(request.HabitId, DateTimeOffset.Parse(request.PerformedAt), request.Comment);
        var logEntry = await _application.CreateLogEntry(command);
        return DtoLogEntry(logEntry.Value);
    }

    public override async Task<PaginatedLogEntries> GetLogEntries(GetLogEntriesRequest request, ServerCallContext context)
    {
        var query = new GetLogEntriesQuery(request.HabitId, CorePeriod(request.SearchPeriod),
            CorePaginationQuery(request.Pagination));
        var logEntries = await _application.GetLogEntries(query);
        return new PaginatedLogEntries
        {
            Data = { logEntries.Value.Data.Select(DtoLogEntry) },
            Pagination = DtoPagination(logEntries.Value.Pagination)
        };
    }

    private static LogEntry DtoLogEntry(Core.LogEntry logEntry)
    {
        return new LogEntry
        {
            Id = logEntry.Id,
            HabitId = logEntry.HabitId,
            Comment = logEntry.Comment,
            PerformedAt = logEntry.PerformedAt.ToString("O")
        };
    }

    private static Habit DtoHabit(Core.Habit habit)
    {
        return new Habit
        {
            Id = habit.Id,
            DirectionId = habit.DirectionId,
            Frequency = habit.Frequency,
            Title = habit.Title,
            ActivePeriod = DtoPeriod(habit)
        };
    }
    
    private static Core.PaginationQuery CorePaginationQuery(PaginationQuery query)
    {
        return new Core.PaginationQuery(query.Offset, query.Count);
    }

    private static PaginationResponse DtoPagination(Core.PaginationResponse pagination)
    {
        return new PaginationResponse
        {
            Count = pagination.Count,
            Offset = pagination.Offset,
            TotalCount = pagination.TotalCount
        };
    }

    private static Core.Period CorePeriod(Period query)
    {
        return new Core.Period(DateTimeOffset.Parse(query.Start),
            DateTimeOffset.Parse(query.End));
    }

    private static Period DtoPeriod(IActivePeriod period)
    {
        return new Period
        {
            Start = period.Start.ToString("O"),
            End = period.End.ToString("O")
        };
    }
    
    private static Direction DtoDirection(Core.Direction direction)
    {
        return new Direction
        {
            Id = direction.Id,
            Title = direction.Title,
            Motivation = direction.Motivation,
            ActivePeriod = DtoPeriod(direction),
            UserProfileId = direction.UserProfileId
        };
    }
}