namespace MultiMart.Application.Identity.Users.ToggleUserStatus;

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