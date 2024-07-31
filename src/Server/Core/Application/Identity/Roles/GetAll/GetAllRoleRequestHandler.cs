namespace MultiMart.Application.Identity.Roles.GetAll;

public class GetAllRoleRequestHandler : IRequestHandler<GetAllRoleRequest, List<RoleDto>>
{
    private readonly IRoleService _roleService;

    public GetAllRoleRequestHandler(IRoleService roleService) => _roleService = roleService;

    public Task<List<RoleDto>> Handle(GetAllRoleRequest request, CancellationToken cancellationToken)
        => _roleService.GetListAsync(cancellationToken);
}