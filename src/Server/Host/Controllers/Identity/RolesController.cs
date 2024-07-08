using MultiMart.Application.Identity.Roles;
using MultiMart.Application.Identity.Roles.Interfaces;
using MultiMart.Application.Identity.Roles.Models;
using MultiMart.Application.Identity.Roles.Requests.Commands;
using MultiMart.Application.Identity.Roles.Requests.Queries;
using MultiMart.Infrastructure.Common.Extensions;
using MultiMart.Shared.Authorization;

namespace MultiMart.Host.Controllers.Identity;

public class RolesController : VersionNeutralApiController
{
    [HttpGet]
    [RequiresPermission(ApplicationAction.View, new[] { ApplicationResource.Roles, ApplicationResource.RoleClaims })]
    [SwaggerOperation("Get a list of all roles.", "")]
    public Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken)
        => Mediator.Send(new GetAllRoleRequest(), cancellationToken);

    [HttpGet("{id}")]
    [RequiresPermission(ApplicationAction.View, new[] { ApplicationResource.Roles, ApplicationResource.RoleClaims })]
    [SwaggerOperation("Get role details.", "")]
    public Task<RoleDto> GetByIdAsync(string id)
        => Mediator.Send(new GetRoleRequest(id));

    [HttpPut("{id}/permissions")]
    [RequiresPermission(ApplicationAction.Update, new[] { ApplicationResource.Roles, ApplicationResource.RoleClaims })]
    [SwaggerOperation("Update a role's permissions.", "")]
    public async Task<string> UpdatePermissionsAsync(string id, UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
        => await Mediator.Send(request.SetPropertyValue(nameof(request.RoleId), id), cancellationToken);

    [HttpPost]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Roles)]
    [SwaggerOperation("Create or update a role.", "")]
    public async Task<string> RegisterRoleAsync(CreateOrUpdateRoleRequest request)
    => await Mediator.Send(request);

    [HttpDelete("{id}")]
    [RequiresPermission(ApplicationAction.Delete, ApplicationResource.Roles)]
    [SwaggerOperation("Delete a role.", "")]
    public async Task<string> DeleteAsync(string id)
        => await Mediator.Send(new DeleteRoleRequest(id));
}