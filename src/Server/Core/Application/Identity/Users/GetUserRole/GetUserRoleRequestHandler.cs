namespace MultiMart.Application.Identity.Users.GetUserRole;

public class GetUserRoleRequestHandler : IRequestHandler<GetUserRoleRequest, List<UserRoleDto>>
{
    private readonly IUserService _userService;

    public GetUserRoleRequestHandler(IUserService userService) => _userService = userService;

    public Task<List<UserRoleDto>> Handle(GetUserRoleRequest request, CancellationToken cancellationToken)
        => _userService.GetRolesAsync(request.Id, cancellationToken);
}