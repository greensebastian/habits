using Habits.Core;
using Habits.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Habits.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddHabitsApplication(this IServiceCollection services)
    {
        services.AddScoped<IHabitsRepository, HabitsRepository>();
        services.AddScoped<IHabitsApplication, HabitsApplication>();
        return services;
    }
}