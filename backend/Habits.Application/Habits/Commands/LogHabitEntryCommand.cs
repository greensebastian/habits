using FluentResults;
using Habits.Core;
using MediatR;

namespace Habits.Application.Habits.Commands;

public record LogHabitEntryCommand(Guid HabitId, DateTimeOffset DoneAt, string? Comment = "") : IRequest<Result<LogEntry>>;