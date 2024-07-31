namespace MultiMart.Application.Identity.Users.SetUserRole;

public class SetUserRolesRequestHandler : IRequestHandler<SetUserRolesRequest, string>
{
    private readonly IUserService _userService;

    public SetUserRolesRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(SetUserRolesRequest request, CancellationToken cancellationToken)
    {
        return await _userService.AssignRolesAsync(request.UserId, request.UserRoles, cancellationToken);
    }
}