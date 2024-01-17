using FluentAssertions;
using Habits.Application;
using Habits.Core.Test.Stubs;

namespace Habits.Core.Test;

public class HabitsApplicationTest
{
    [Fact]
    public async Task HabitsApplication_AddAndGetProfile_ReturnsProfile()
    {
        // Arrange
        var repository = new HabitsRepositoryStub() as IHabitsRepository;
        await repository.Add(new UserProfile
        {
            Id = "id",
            Name = "name"
        }, null);

        // Act
        var sut = new HabitsApplication(repository);
        var actual = await sut.GetUserProfile(new GetUserProfileQuery("id"), null);

        // Assert
        actual.IsSuccessful.Should().BeTrue();
        actual.Value.Name.Should().Be("name");
        actual.Value.Id.Should().Be("id");
    }

    [Fact]
    public async Task HabitsApplication_GetNonExistingProfile_ReturnsException()
    {
        // Arrange
        var repository = new HabitsRepositoryStub() as IHabitsRepository;
        
        // Act
        var sut = new HabitsApplication(repository);
        var actual = await sut.GetUserProfile(new GetUserProfileQuery("id"), null);

        // Assert
        actual.IsSuccessful.Should().BeFalse();
        actual.Error.Should().BeOfType<NotFoundException>();
    }
}