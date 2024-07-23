using System.Text.Json.Serialization;
using MultiMart.Application.Identity.Users.Interfaces;

namespace MultiMart.Application.Identity.Users.Requests;

public class ToggleUserStatusRequest : IRequest<Unit>
{
    public bool ActivateUser { get; set; }
    [JsonIgnore]
    public string? UserId { get; set; }
}

public class ToggleUserStatusRequestHandler
{
    private readonly IUserService _userService;

    public ToggleUserStatusRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Unit> Handle(ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        await _userService.ToggleStatusAsync(request.ActivateUser, request.UserId!, cancellationToken);
        return Unit.Value;
    }
}
