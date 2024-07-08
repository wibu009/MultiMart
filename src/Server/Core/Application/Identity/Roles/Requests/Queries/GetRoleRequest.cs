using MultiMart.Application.Identity.Roles.Interfaces;
using MultiMart.Application.Identity.Roles.Models;

namespace MultiMart.Application.Identity.Roles.Requests.Queries;

public class GetRoleRequest : IRequest<RoleDto>
{
    public string Id { get; set; }

    public GetRoleRequest(string id)
    {
        Id = id;
    }
}

public class GetRoleRequestHandler : IRequestHandler<GetRoleRequest, RoleDto>
{
    private readonly IRoleService _roleService;

    public GetRoleRequestHandler(IRoleService roleService) => _roleService = roleService;

    public Task<RoleDto> Handle(GetRoleRequest request, CancellationToken cancellationToken)
        => _roleService.GetByIdAsync(request.Id);
}