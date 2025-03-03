using FluentResults;
using Habits.Application.Paginations;
using Habits.Core;

namespace Habits.Application.Habits.Dependencies;

public interface IHabitsRepository
{
    public Result Add(Habit habit);
    
    public Task<Result<Paginated<Habit>>> Get(PaginationRequest request, CancellationToken cancellationToken);
    
    public Task<Result<Habit>> Get(Guid habitId, CancellationToken cancellationToken);

    public Task<Result> Save(CancellationToken cancellationToken);
}