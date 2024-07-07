using MultiMart.Application.Identity.Users.Interfaces;
using MultiMart.Application.Identity.Users.Models;

namespace MultiMart.Application.Identity.Users.Requests.Queries;

public class GetUserRoleRequest : IRequest<List<UserRoleDto>>
{
    public string Id { get; set; }

    public GetUserRoleRequest(string id)
    {
        Id = id;
    }
}

public class GetUserRoleRequestHandler : IRequestHandler<GetUserRoleRequest, List<UserRoleDto>>
{
    private readonly IUserService _userService;

    public GetUserRoleRequestHandler(IUserService userService) => _userService = userService;

    public Task<List<UserRoleDto>> Handle(GetUserRoleRequest request, CancellationToken cancellationToken)
        => _userService.GetRolesAsync(request.Id, cancellationToken);
}