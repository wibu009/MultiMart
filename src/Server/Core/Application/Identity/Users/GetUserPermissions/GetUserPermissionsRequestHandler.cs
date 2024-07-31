namespace MultiMart.Application.Identity.Users.GetUserPermissions;

public class GetUserPermissionsRequestHandler : IRequestHandler<GetUserPermissionsRequest, List<string>>
{
    private readonly IUserService _userService;

    public GetUserPermissionsRequestHandler(IUserService userService) => _userService = userService;

    public Task<List<string>> Handle(GetUserPermissionsRequest request, CancellationToken cancellationToken)
        => _userService.GetPermissionsAsync(request.Id, cancellationToken);
}