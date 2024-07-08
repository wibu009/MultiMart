using MultiMart.Application.Identity.Roles.Interfaces;

namespace MultiMart.Application.Identity.Roles.Requests.Commands;

public class DeleteRoleRequest : IRequest<string>
{
    public string Id { get; set; }

    public DeleteRoleRequest(string id)
    {
        Id = id;
    }
}

public class DeleteRoleRequestHandler : IRequestHandler<DeleteRoleRequest, string>
{
    private readonly IRoleService _roleService;

    public DeleteRoleRequestHandler(IRoleService roleService) => _roleService = roleService;

    public Task<string> Handle(DeleteRoleRequest request, CancellationToken cancellationToken) =>
        _roleService.DeleteAsync(request.Id);
}