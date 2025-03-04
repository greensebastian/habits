using FluentResults;
using Habits.Application.Habits.Dependencies;
using Habits.Application.Paginations;
using Habits.Core;
using Microsoft.EntityFrameworkCore;

namespace Habits.Infrastructure;

public class HabitsRepository(HabitsDbContext dbContext) : IHabitsRepository
{
    public Result Add(Habit habit) => Result.Try(() => dbContext.Add(habit)).ToResult();

    public async Task<Result<Paginated<Habit>>> Get(PaginationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var count = await dbContext.Habits.CountAsync(cancellationToken);
                
            var habits = await dbContext.Habits
                .OrderByDescending(habit => habit.CreatedAt)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(cancellationToken);

            return request.Response(habits, count);
        }
        catch (Exception ex)
        {
            return new Error("Failed to get habits from database").CausedBy(ex);
        }
    }

    public async Task<Result<Habit>> Get(Guid habitId, CancellationToken cancellationToken)
    {
        try
        {
            var habit = await dbContext.Habits.SingleOrDefaultAsync(habit => habit.Id == habitId, cancellationToken);
            return habit is not null ? habit : Result.Fail("Not found");
        }
        catch (Exception ex)
        {
            return new Error("Failed to get habit from database").CausedBy(ex);
        }
    }

    public async Task<Result> Save(CancellationToken cancellationToken)
    {
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return new Error("Failed to save changes to database").CausedBy(ex);
        }
    }
}