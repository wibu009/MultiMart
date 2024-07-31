namespace MultiMart.Application.Identity.Roles.Delete;

public class DeleteRoleRequestHandler : IRequestHandler<DeleteRoleRequest, string>
{
    private readonly IRoleService _roleService;

    public DeleteRoleRequestHandler(IRoleService roleService) => _roleService = roleService;

    public Task<string> Handle(DeleteRoleRequest request, CancellationToken cancellationToken) =>
        _roleService.DeleteAsync(request.Id);
}