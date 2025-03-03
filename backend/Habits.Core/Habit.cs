using FluentResults;

namespace Habits.Core;

public class Habit
{
    private Habit(){}
    
    private readonly List<LogEntry> _entries = [];
    
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public IReadOnlyList<LogEntry> Entries => _entries;
    public required DateTimeOffset CreatedAt { get; init; }

    public static Result<Habit> Create(string name, DateTimeOffset createdAt)
    {
        if (string.IsNullOrWhiteSpace(name)) return Result.Fail("Habits must have a name");
        
        var habit = new Habit
        {
            Id = Guid.CreateVersion7(createdAt),
            Name = name,
            CreatedAt = createdAt
        };

        return habit;
    }

    public Result<LogEntry> Log(DateTimeOffset doneAt, string? comment)
    {
        if (comment is not null && string.IsNullOrWhiteSpace(comment)) return Result.Fail("Log comments must not be empty or whitespace");
        
        var entry = new LogEntry
        {
            Id = Guid.CreateVersion7(doneAt),
            DoneAt = doneAt,
            Comment = comment
        };
        
        var position = _entries.FindLastIndex(e => e.DoneAt <= doneAt);
        if (position <= 0) _entries.Insert(position + 1, entry);
        else _entries.Add(entry);

        return entry;
    }
}

public class LogEntry
{
    internal LogEntry(){}
    
    public required Guid Id { get; init; }
    public required DateTimeOffset DoneAt { get; init; }
    public string? Comment { get; init; }
}