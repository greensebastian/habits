using Grpc.Core;
using Habits.Core;

namespace Habits.gRPC.Services;

public class HabitsService : gRPC.HabitsService.HabitsServiceBase
{
    private readonly IHabitsApplication _application;

    public HabitsService(IHabitsApplication application)
    {
        _application = application;
    }
    
    public override async Task<UserProfile> CreateProfile(CreateProfileRequest request, ServerCallContext context)
    {
        var command = new CreateUserProfileCommand(request.Name);
        var userProfile = await _application.CreateUserProfile(command);
        return new UserProfile
        {
            Id = userProfile.Value.Id,
            Name = userProfile.Value.Name
        };
    }

    public override async Task<Direction> CreateDirection(CreateDirectionRequest request, ServerCallContext context)
    {
        var command = new CreateDirectionCommand(request.UserProfileId, request.Title, request.Motivation,
            new Period(DateTimeOffset.Parse(request.Start), DateTimeOffset.Parse(request.End)));
        var direction = await _application.CreateDirection(command);
        return new Direction
        {
            Id = direction.Value.Id,
            Title = direction.Value.Title,
            Motivation = direction.Value.Motivation,
            Start = direction.Value.Start.ToString("O"),
            End = direction.Value.End.ToString("O"),
            UserProfileId = direction.Value.UserProfileId
        };
    }
}