using FluentAssertions;
using Habits.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Habits.IntegrationTest;

public class IntegrationTest : IClassFixture<IntegrationTestFixture>, IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;

    public IntegrationTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Can_Create_UserProfile()
    {
        await _fixture.WithScope(async services =>
        {
            // Arrange
            var createCommand = new CreateUserProfileCommand("Seb");
            
            // Act
            var sut = services.GetRequiredService<IHabitsApplication>();
            var createdProfile = await sut.CreateUserProfile(createCommand);
            var actual = await sut.GetUserProfile(new GetUserProfileQuery(createdProfile.Value.Id), default);
            
            // Assert
            Guid.Parse(actual.Value.Id).Should().NotBeEmpty().And.NotBe(Guid.Empty);
            actual.Value.Name.Should().Be("Seb");
        });
    }
    
    [Fact]
    public async Task Can_Create_Direction()
    {
        await _fixture.WithScope(async services =>
        {
            // Arrange
            var sut = services.GetRequiredService<IHabitsApplication>();
            var executionTime = DateTimeOffset.Parse("2024-01-19T12:00:00Z");
            var userProfile = await _fixture.CreateUserProfile(new CreateUserProfileCommand("Seb"));
            var createDirectionCommand = new CreateDirectionCommand(userProfile.Id, "Title", "Motivation",
                new Period(executionTime.AddDays(-1), executionTime.AddDays(1)));
            
            // Act
            var createdDirection = await sut.CreateDirection(createDirectionCommand);
            var actual = await sut.GetDirections(new GetDirectionsQuery(userProfile.Id,
                new Period(executionTime, executionTime.AddHours(1)), new PaginationQuery()));
            
            // Assert
            actual.Value.Data.Should().HaveCount(1);
            var actualDirection = actual.Value.Data[0];
            actualDirection.Id.Should().Be(createdDirection.Value.Id);
            Guid.Parse(actualDirection.Id).Should().NotBeEmpty().And.NotBe(Guid.Empty);
            actualDirection.Title.Should().Be("Title");
            actualDirection.Motivation.Should().Be("Motivation");
            actualDirection.Start.Should().Be(executionTime.AddDays(-1));
            actualDirection.End.Should().Be(executionTime.AddDays(1));
        });
    }
    
    [Fact]
    public async Task Direction_Pagination()
    {
        var userProfile = await _fixture.CreateUserProfile(new CreateUserProfileCommand("User"));
        
        await _fixture.WithScope(async services =>
        {
            // Arrange
            var sut = services.GetRequiredService<IHabitsApplication>();
            var executionTime = DateTimeOffset.Parse("2024-01-19T12:00:00Z");
            var activePeriod = new Period(executionTime.AddDays(-1), executionTime.AddDays(1));
            for (var i = 0; i < 20; i++)
            {
                await sut.CreateDirection(new CreateDirectionCommand(userProfile.Id, $"Title-{i:0}",
                    $"Motivation-{i:0}", activePeriod));
            }
            
            // Act
            var actualOffset = await sut.GetDirections(new GetDirectionsQuery(userProfile.Id,
                new Period(executionTime, executionTime.AddHours(1)), new PaginationQuery(5, 100)));
            
            var actualCounted = await sut.GetDirections(new GetDirectionsQuery(userProfile.Id,
                new Period(executionTime, executionTime.AddHours(1)), new PaginationQuery(0, 10)));
            
            // Assert
            actualOffset.Value.Pagination.Offset.Should().Be(5);
            actualOffset.Value.Pagination.Count.Should().Be(15);
            actualOffset.Value.Pagination.TotalCount.Should().Be(20);
            
            actualCounted.Value.Pagination.Offset.Should().Be(0);
            actualCounted.Value.Pagination.Count.Should().Be(10);
            actualCounted.Value.Pagination.TotalCount.Should().Be(20);
        });
    }
    
    [Fact]
    public async Task Direction_DateFilter()
    {
        // Arrange
        var executionTime = DateTimeOffset.Parse("2024-01-19T12:00:00Z");
        var activePeriod = new Period(executionTime, executionTime.AddDays(10));
        var userProfile = await _fixture.CreateUserProfile(new CreateUserProfileCommand("User"));
        var direction =
            await _fixture.CreateDirection(new CreateDirectionCommand(userProfile.Id, "Title", "Motivation",
                activePeriod));

        await _fixture.WithScope(async services =>
        {
            var sut = services.GetRequiredService<IHabitsApplication>();
            var beforeQuery = new GetDirectionsQuery(userProfile.Id,
                new Period(activePeriod.Start.AddDays(-5), activePeriod.Start.AddDays(-1)), new PaginationQuery());
            var overlapStartQuery = new GetDirectionsQuery(userProfile.Id,
                new Period(activePeriod.Start.AddDays(-1), activePeriod.Start.AddDays(1)), new PaginationQuery());
            var containedQuery = new GetDirectionsQuery(userProfile.Id,
                new Period(activePeriod.Start.AddDays(1), activePeriod.End.AddDays(-1)), new PaginationQuery());
            var coveredQuery = new GetDirectionsQuery(userProfile.Id,
                new Period(activePeriod.Start.AddDays(-1), activePeriod.End.AddDays(1)), new PaginationQuery());
            var overlapEndQuery = new GetDirectionsQuery(userProfile.Id,
                new Period(activePeriod.End.AddDays(-1), activePeriod.End.AddDays(1)), new PaginationQuery());
            var afterQuery = new GetDirectionsQuery(userProfile.Id,
                new Period(activePeriod.End.AddDays(1), activePeriod.End.AddDays(5)), new PaginationQuery());

            // Act & Assert
            foreach (var query in new[] { beforeQuery, afterQuery })
            {
                var actual = await sut.GetDirections(query);
                actual.Value.Data.Should().BeEmpty();
            }

            foreach (var query in new[] { overlapStartQuery, containedQuery, coveredQuery, overlapEndQuery })
            {
                var actual = await sut.GetDirections(query);
                actual.Value.Data.Should().ContainSingle();
                actual.Value.Data[0].Id.Should().Be(direction.Id);
            }
        });
    }

    [Fact]
    public async Task Can_Create_Habit()
    {
        // Arrange
        var executionTime = DateTimeOffset.Parse("2024-01-19T12:00:00Z");
        var activePeriod = new Period(executionTime, executionTime.AddDays(10));
        var userProfile = await _fixture.CreateUserProfile(new CreateUserProfileCommand("User"));
        var direction =
            await _fixture.CreateDirection(new CreateDirectionCommand(userProfile.Id, "Title", "Motivation",
                activePeriod));
        
        await _fixture.WithScope(async services =>
        {
            // Arrange
            var sut = services.GetRequiredService<IHabitsApplication>();
            var createHabitCommand = new CreateHabitCommand(direction.Id, "Title", "Frequency", activePeriod);
            
            // Act
            var createdHabit = await sut.CreateHabit(createHabitCommand);
            var actual = await sut.GetHabits(new GetHabitsQuery(direction.Id,
                new Period(executionTime, executionTime.AddHours(1)), new PaginationQuery()));
            
            // Assert
            actual.Value.Data.Should().HaveCount(1);
            var actualHabit = actual.Value.Data[0];
            actualHabit.Id.Should().Be(createdHabit.Value.Id);
            Guid.Parse(actualHabit.Id).Should().NotBeEmpty().And.NotBe(Guid.Empty);
            actualHabit.Title.Should().Be("Title");
            actualHabit.Frequency.Should().Be("Frequency");
            actualHabit.DirectionId.Should().Be(direction.Id);
            actualHabit.Start.Should().Be(executionTime);
            actualHabit.End.Should().Be(executionTime.AddDays(10));
        });
    }
    
    [Fact]
    public async Task Can_Create_LogEntry()
    {
        // Arrange
        var executionTime = DateTimeOffset.Parse("2024-01-19T12:00:00Z");
        var activePeriod = new Period(executionTime, executionTime.AddDays(10));
        var userProfile = await _fixture.CreateUserProfile(new CreateUserProfileCommand("User"));
        var direction =
            await _fixture.CreateDirection(new CreateDirectionCommand(userProfile.Id, "Title", "Motivation",
                activePeriod));
        var habit = await _fixture.CreateHabit(new CreateHabitCommand(direction.Id, "Title", "Frequency",
            activePeriod));
        
        await _fixture.WithScope(async services =>
        {
            // Arrange
            var sut = services.GetRequiredService<IHabitsApplication>();
            var createLogEntryCommand = new CreateLogEntryCommand(habit.Id, executionTime.AddDays(1), "Comment");
            
            // Act
            var createdLogEntry = await sut.CreateLogEntry(createLogEntryCommand);
            var actual = await sut.GetLogEntries(new GetLogEntriesQuery(habit.Id,
                new Period(executionTime, executionTime.AddDays(2)), new PaginationQuery()));
            
            // Assert
            actual.Value.Data.Should().HaveCount(1);
            var actualLogEntry = actual.Value.Data[0];
            actualLogEntry.Id.Should().Be(createdLogEntry.Value.Id);
            Guid.Parse(actualLogEntry.Id).Should().NotBeEmpty().And.NotBe(Guid.Empty);
            actualLogEntry.Comment.Should().Be("Comment");
            actualLogEntry.PerformedAt.Should().Be(executionTime.AddDays(1));
        });
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _fixture.Clean();
    }
}