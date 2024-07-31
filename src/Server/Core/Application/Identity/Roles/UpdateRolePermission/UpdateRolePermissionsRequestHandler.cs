namespace MultiMart.Application.Identity.Roles.UpdateRolePermission;

public class UpdateRolePermissionsRequestHandler : IRequestHandler<UpdateRolePermissionsRequest, string>
{
    private readonly IRoleService _roleService;

    public UpdateRolePermissionsRequestHandler(IRoleService roleService) => _roleService = roleService;

    public Task<string> Handle(UpdateRolePermissionsRequest request, CancellationToken cancellationToken) =>
        _roleService.UpdatePermissionsAsync(request.RoleId, request.Permissions);
}