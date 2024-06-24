using MultiMart.Application.Identity.Roles;
using MultiMart.Infrastructure.Common.Extensions;
using MultiMart.Shared.Authorization;

namespace MultiMart.Host.Controllers.Identity;

public class RolesController : VersionNeutralApiController
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService) => _roleService = roleService;

    [HttpGet]
    [RequiresPermission(ApplicationAction.View, new[] { ApplicationResource.Roles, ApplicationResource.RoleClaims })]
    [SwaggerOperation("Get a list of all roles.", "")]
    public Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return _roleService.GetListAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    [RequiresPermission(ApplicationAction.View, new[] { ApplicationResource.Roles, ApplicationResource.RoleClaims })]
    [SwaggerOperation("Get role details.", "")]
    public Task<RoleDto> GetByIdAsync(string id)
    {
        return _roleService.GetByIdAsync(id);
    }

    [HttpPut("{id}/permissions")]
    [RequiresPermission(ApplicationAction.Update, new[] { ApplicationResource.Roles, ApplicationResource.RoleClaims })]
    [SwaggerOperation("Update a role's permissions.", "")]
    public async Task<ActionResult<string>> UpdatePermissionsAsync(string id, UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _roleService.UpdatePermissionsAsync(request.SetPropertyValue(nameof(UpdateRolePermissionsRequest.RoleId), id), cancellationToken));
    }

    [HttpPost]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Roles)]
    [SwaggerOperation("Create or update a role.", "")]
    public Task<string> RegisterRoleAsync(CreateOrUpdateRoleRequest request)
    {
        return _roleService.CreateOrUpdateAsync(request);
    }

    [HttpDelete("{id}")]
    [RequiresPermission(ApplicationAction.Delete, ApplicationResource.Roles)]
    [SwaggerOperation("Delete a role.", "")]
    public Task<string> DeleteAsync(string id)
    {
        return _roleService.DeleteAsync(id);
    }
}