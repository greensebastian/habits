using FluentResults;
using Habits.Application.Habits.Dependencies;
using Habits.Core;
using MediatR;

namespace Habits.Application.Habits.Commands;

public record CreateHabitCommand(string Name) : IRequest<Result<Habit>>;

public class CreateHabitHandler(TimeProvider timeProvider, IHabitsRepository habitsRepository) : IRequestHandler<CreateHabitCommand, Result<Habit>>
{
    public async Task<Result<Habit>> Handle(CreateHabitCommand request, CancellationToken cancellationToken)
    {
        var habit = Habit.Create(request.Name, timeProvider.GetUtcNow());
        if (habit.IsFailed) return habit;

        var addResult = habitsRepository.Add(habit.Value);
        if (addResult.IsFailed) return addResult;

        var save = await habitsRepository.Save(cancellationToken);
        return save.IsSuccess ? habit : save;
    }
}