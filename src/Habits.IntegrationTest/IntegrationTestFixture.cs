using Habits.Application;
using Habits.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Habits.IntegrationTest;

public class IntegrationTestFixture : IAsyncLifetime
{
    private const string PostgresUser = "postgres";
    private const string PostgresPassword = "password";
    private const string PostgresDb = "IntegrationTests";

    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithUsername(PostgresUser)
        .WithPassword(PostgresPassword)
        .WithDatabase(PostgresDb)
        .Build();
    
    public IHost TestHost { get; private set; }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        var inMemoryConfigSource = new MemoryConfigurationSource
        {
            InitialData = new Dictionary<string, string?>
            {
                { "ConnectionStrings:Default", _postgreSqlContainer.GetConnectionString() }
            }
        };
        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureAppConfiguration(config =>
        {
            config.Add(inMemoryConfigSource);
        });
        builder.ConfigureServices(services =>
        {
            services.AddHabitsApplication();
        });

        TestHost = builder.Build();

        await WithScope(async services =>
        {
            var context = services.GetRequiredService<HabitsContext>();
            await context.Database.MigrateAsync();
        });
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
    
    public async Task WithScope(Func<IServiceProvider, Task> action)
    {
        await using var scope = TestHost.Services.CreateAsyncScope();
        await action(scope.ServiceProvider);
    }

    public async Task Clean()
    {
        await WithScope(async services =>
        {
            var context = services.GetRequiredService<HabitsContext>();
            var transaction = await context.Database.BeginTransactionAsync();

            await context.LogEntries.ExecuteDeleteAsync();
            await context.Habits.ExecuteDeleteAsync();
            await context.Directions.ExecuteDeleteAsync();
            await context.UserProfiles.ExecuteDeleteAsync();

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        });
    }
}