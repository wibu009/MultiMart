using System.Text.Json.Serialization;
using MultiMart.Application.Identity.Roles.Interfaces;

namespace MultiMart.Application.Identity.Roles.Requests.Commands;

public class UpdateRolePermissionsRequest : IRequest<string>
{
    [JsonIgnore]
    public string RoleId { get; set; } = default!;
    public List<string> Permissions { get; set; } = default!;
}

public class UpdateRolePermissionsRequestHandler : IRequestHandler<UpdateRolePermissionsRequest, string>
{
    private readonly IRoleService _roleService;

    public UpdateRolePermissionsRequestHandler(IRoleService roleService) => _roleService = roleService;

    public Task<string> Handle(UpdateRolePermissionsRequest request, CancellationToken cancellationToken) =>
        _roleService.UpdatePermissionsAsync(request.RoleId, request.Permissions);
}