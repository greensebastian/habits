using FluentResults;
using Habits.Application.Habits.Commands;
using Habits.Application.Habits.Dependencies;
using Habits.Application.Habits.Queries;
using Habits.Application.Paginations;
using Habits.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Habits.WebApi;

public static class WebApplicationExtensions
{
    public static WebApplicationBuilder AddHabits(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddProblemDetails();
        
        builder.Services.AddOpenApi();
        
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<HabitsQuery>());

        builder.Services.Configure<HabitsDbSettings>(builder.Configuration.GetSection("HabitsDb"));
        builder.Services.AddDbContext<HabitsDbContext>();
        builder.Services.AddScoped<IHabitsRepository, HabitsRepository>();
        builder.Services.AddHostedService<HabitsDbContextMigrator>();
        return builder;
    }

    public static WebApplication UseHabits(this WebApplication app)
    {
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
        app.MapOpenApi();

        app.UseHttpsRedirection();

        app.MapGet("/habits", async ([FromServices] IMediator mediator, int offset = 0, int limit = 10) =>
        {
            var result = await mediator.Send(new HabitsQuery(new PaginationRequest(offset, limit)));
            if (result.IsSuccess) return Results.Ok(result.Value);
            return result.Problem();
        });
        
        app.MapPost("/habits", async ([FromServices] IMediator mediator, CreateHabitCommand request) =>
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess) return Results.Ok(result.Value);
            return result.Problem();
        });
        
        app.MapPost("/habits/{habitId}/logEntries", async ([FromServices] IMediator mediator, [FromRoute] Guid habitId, LogHabitEntryRequest request) =>
        {
            var result = await mediator.Send(request.AsCommand(habitId));
            if (result.IsSuccess) return Results.Ok(result.Value);
            return result.Problem();
        });

        app.UseStatusCodePages();
        
        return app;
    }

    private static IResult Problem(this Result result) => Results.Problem(new()
    {
        Title = "Error",
        Status = StatusCodes.Status500InternalServerError,
        Extensions =
        {
            { "errors", result.Errors }
        }
    });
    
    private static IResult Problem<T>(this Result<T> result) => result.ToResult().Problem();
}

public record LogHabitEntryRequest(DateTimeOffset DoneAt, string? Comment = null)
{
    public LogHabitEntryCommand AsCommand(Guid id) => new(id, DoneAt, Comment);
}