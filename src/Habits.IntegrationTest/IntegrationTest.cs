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
            var createUserProfileCommand = new CreateUserProfileCommand("Seb");
            var createdUserProfile = await sut.CreateUserProfile(createUserProfileCommand);
            var createDirectionCommand = new CreateDirectionCommand(createdUserProfile.Value.Id, "Title", "Motivation",
                new Period(executionTime.AddDays(-1), executionTime.AddDays(1)));
            
            // Act
            var createdDirection = await sut.CreateDirection(createDirectionCommand);
            var actual = await sut.GetDirections(new GetDirectionsQuery(createdUserProfile.Value.Id,
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

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _fixture.Clean();
    }
}