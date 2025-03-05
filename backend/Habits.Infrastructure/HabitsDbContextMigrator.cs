using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Habits.Infrastructure;

public class HabitsDbContextMigrator(IServiceProvider serviceProvider, IOptions<HabitsDbSettings> settings) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!settings.Value.MigrateOnStartup) return;
        
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HabitsDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}