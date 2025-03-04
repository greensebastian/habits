using FluentResults;
using Habits.Application.Habits.Dependencies;
using Habits.Application.Paginations;
using Habits.Core;
using MediatR;

namespace Habits.Application.Habits.Queries;

public record HabitsQuery(PaginationRequest Pagination) : IRequest<Result<Paginated<Habit>>>;

public class HabitsQueryHandler(IHabitsRepository repository) : IRequestHandler<HabitsQuery, Result<Paginated<Habit>>>
{
    public async Task<Result<Paginated<Habit>>> Handle(HabitsQuery request, CancellationToken cancellationToken)
    {
        return await repository.Get(request.Pagination, cancellationToken);
    }
}