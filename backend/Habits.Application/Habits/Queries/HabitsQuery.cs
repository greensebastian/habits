using FluentResults;
using Habits.Application.Paginations;
using Habits.Core;
using MediatR;

namespace Habits.Application.Habits.Queries;

public record HabitsQuery(PaginationRequest Pagination) : IRequest<Result<Paginated<Habit>>>;