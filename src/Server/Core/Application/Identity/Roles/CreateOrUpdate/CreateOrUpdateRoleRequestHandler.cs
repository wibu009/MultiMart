namespace MultiMart.Application.Identity.Roles.CreateOrUpdate;

public class CreateOrUpdateRoleRequestHandler : IRequestHandler<CreateOrUpdateRoleRequest, string>
{
    private readonly IRoleService _roleService;

    public CreateOrUpdateRoleRequestHandler(IRoleService roleService) => _roleService = roleService;

    public Task<string> Handle(CreateOrUpdateRoleRequest request, CancellationToken cancellationToken) =>
        _roleService.CreateOrUpdateAsync(request.Id, request.Name, request.Description);
}