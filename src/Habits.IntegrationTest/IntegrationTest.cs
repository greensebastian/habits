using FluentAssertions;
using Habits.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Habits.IntegrationTest;

public class IntegrationTest : IClassFixture<IntegrationTestFixture>
{
    private readonly IntegrationTestFixture _fixture;

    public IntegrationTest(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    private async Task ExecuteInScope(Func<IServiceProvider, Task> action)
    {
        await using var scope = _fixture.TestHost.Services.CreateAsyncScope();
        await action(scope.ServiceProvider);
    }

    [Fact]
    public async Task Can_Create_UserProfile()
    {
        await ExecuteInScope(async services =>
        {
            // Arrange
            var createCommand = new CreateUserProfileCommand("Seb");
            
            // Act
            var sut = services.GetRequiredService<IHabitsApplication>();
            var createdProfile = await sut.CreateUserProfile(createCommand, default);
            var actual = await sut.GetUserProfile(new GetUserProfileQuery(createdProfile.Value.Id), default);
            
            // Assert
            Guid.Parse(actual.Value.Id).Should().NotBeEmpty().And.NotBe(Guid.Empty);
            actual.Value.Name.Should().Be("Seb");
        });
    }
}