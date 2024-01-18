using Habits.Application;
using Habits.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Habits.IntegrationTest;

[Collection(Collections.Integration)]
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
        
        await using var scope = TestHost.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<HabitsContext>();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
}