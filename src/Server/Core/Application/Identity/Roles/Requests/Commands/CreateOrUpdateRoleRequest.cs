using MultiMart.Application.Identity.Roles.Interfaces;

namespace MultiMart.Application.Identity.Roles.Requests.Commands;

public class CreateOrUpdateRoleRequest : IRequest<string>
{
    public string? Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}

public class CreateOrUpdateRoleRequestHandler : IRequestHandler<CreateOrUpdateRoleRequest, string>
{
    private readonly IRoleService _roleService;

    public CreateOrUpdateRoleRequestHandler(IRoleService roleService) => _roleService = roleService;

    public Task<string> Handle(CreateOrUpdateRoleRequest request, CancellationToken cancellationToken) =>
        _roleService.CreateOrUpdateAsync(request.Id, request.Name, request.Description);
}