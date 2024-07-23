using System.Text.Json.Serialization;
using MultiMart.Application.Identity.Users.Interfaces;
using MultiMart.Application.Identity.Users.Models;

namespace MultiMart.Application.Identity.Users.Requests;

public class UserRolesRequest : IRequest<string>
{
    [JsonIgnore]
    public string UserId { get; set; } = default!;
    public List<UserRoleDto> UserRoles { get; set; } = new();
}

public class UserRolesRequestHandler : IRequestHandler<UserRolesRequest, string>
{
    private readonly IUserService _userService;

    public UserRolesRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(UserRolesRequest request, CancellationToken cancellationToken)
    {
        return await _userService.AssignRolesAsync(request.UserId, request.UserRoles, cancellationToken);
    }
}
