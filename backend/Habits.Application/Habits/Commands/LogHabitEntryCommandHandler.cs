using FluentResults;
using Habits.Application.Habits.Dependencies;
using Habits.Core;
using MediatR;

namespace Habits.Application.Habits.Commands;

public class LogHabitEntryCommandHandler(IHabitsRepository habitsRepository) : IRequestHandler<LogHabitEntryCommand, Result<LogEntry>>
{
    public async Task<Result<LogEntry>> Handle(LogHabitEntryCommand request, CancellationToken cancellationToken)
    {
        var habit = await habitsRepository.Get(request.HabitId, cancellationToken);
        if (habit.IsFailed) return habit.ToResult();

        var logEntry = habit.Value.Log(request.DoneAt, request.Comment);
        if (logEntry.IsFailed) return logEntry;

        var saveResult = await habitsRepository.Save(cancellationToken);
        return saveResult.IsFailed ? saveResult : logEntry;
    }
}