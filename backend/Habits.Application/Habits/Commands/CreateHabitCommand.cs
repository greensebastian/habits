using FluentResults;
using Habits.Core;
using MediatR;

namespace Habits.Application.Habits.Commands;

public record CreateHabitCommand(string Name) : IRequest<Result<Habit>>;