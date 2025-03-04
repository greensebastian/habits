namespace Habits.Core;

public class LogEntry
{
    internal LogEntry(){}
    
    public required Guid Id { get; init; }
    public required DateTimeOffset DoneAt { get; init; }
    public string? Comment { get; init; }
}