using System.Security.Claims;
using Finbuckle.MultiTenant;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MultiMart.Application.Common.Events;
using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Identity.Roles;
using MultiMart.Application.Identity.Roles.Interfaces;
using MultiMart.Application.Identity.Roles.Models;
using MultiMart.Application.Identity.Roles.Requests.Commands;
using MultiMart.Domain.Identity;
using MultiMart.Infrastructure.Identity.User;
using MultiMart.Shared.Authorization;
using MultiMart.Shared.Multitenancy;

namespace MultiMart.Infrastructure.Identity.Role;

internal class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer _t;
    private readonly ITenantInfo _currentTenant;
    private readonly IEventPublisher _events;

    public RoleService(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IStringLocalizer<RoleService> localizer,
        ITenantInfo currentTenant,
        IEventPublisher events)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _t = localizer;
        _currentTenant = currentTenant;
        _events = events;
    }

    public async Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken) =>
        (await _roleManager.Roles.Include(r => r.RoleClaims).ToListAsync(cancellationToken))
            .Adapt<List<RoleDto>>();

    public async Task<int> GetCountAsync(CancellationToken cancellationToken) =>
        await _roleManager.Roles.CountAsync(cancellationToken);

    public async Task<bool> ExistsAsync(string roleName, string? excludeId) =>
        await _roleManager.FindByNameAsync(roleName)
            is ApplicationRole existingRole
            && existingRole.Id != excludeId;

    public async Task<RoleDto> GetByIdAsync(string id) =>
        await _roleManager.Roles.Include(r => r.RoleClaims).FirstOrDefaultAsync(r => r.Id == id) is ApplicationRole role
            ? role.Adapt<RoleDto>()
            : throw new NotFoundException(_t["Role Not Found"]);

    public async Task<string> CreateOrUpdateAsync(string? id, string name, string? description)
    {
        if (string.IsNullOrEmpty(id))
        {
            // Create a new role.
            var role = new ApplicationRole
            {
                Name = name,
                NormalizedName = name.ToUpperInvariant(),
                Description = description
            };
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new InternalServerException(_t["Register role failed"], result.GetErrors(_t));
            }

            await _events.PublishAsync(new ApplicationRoleCreatedEvent(role.Id, role.Name!));

            return string.Format(_t["Role {0} Created."], name);
        }
        else
        {
            // Update an existing role.
            var role = await _roleManager.FindByIdAsync(id);

            _ = role ?? throw new NotFoundException(_t["Role Not Found"]);

            if (ApplicationRoles.IsDefault(role.Name!))
            {
                throw new ConflictException(string.Format(_t["Not allowed to modify {0} Role."], role.Name));
            }

            role.Name = name;
            role.NormalizedName = name.ToUpperInvariant();
            role.Description = description;
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                throw new InternalServerException(_t["Update role failed"], result.GetErrors(_t));
            }

            await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name));

            return string.Format(_t["Role {0} Updated."], role.Name);
        }
    }

    public async Task<string> UpdatePermissionsAsync(string roleId, List<string> permissions)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        _ = role ?? throw new NotFoundException(_t["Role Not Found"]);

        if (role.Name == ApplicationRoles.Admin)
        {
            throw new ConflictException(_t["Not allowed to modify Permissions for this Role."]);
        }

        if (_currentTenant.Id != MultitenancyConstants.Root.Id)
        {
            // Remove Root Permissions if the Role is not created for Root Tenant.
            permissions.RemoveAll(u => u.StartsWith("Permissions.Root."));
        }

        var currentClaims = await _roleManager.GetClaimsAsync(role);

        // Remove permissions that were previously selected
        foreach (var claim in currentClaims.Where(c => permissions.All(p => p != c.Value)))
        {
            var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
            if (!removeResult.Succeeded)
            {
                throw new InternalServerException(_t["Update permissions failed."], removeResult.GetErrors(_t));
            }
        }

        // Add all permissions that were not previously selected
        foreach (string permission in permissions.Where(c => currentClaims.All(p => p.Value != c)))
        {
            if (!string.IsNullOrEmpty(permission))
            {
                await _roleManager.AddClaimAsync(role, new Claim(ApplicationClaims.Permission, permission));
            }
        }

        await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name!, true));

        return _t["Permissions Updated."];
    }

    public async Task<string> DeleteAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        _ = role ?? throw new NotFoundException(_t["Role Not Found"]);

        if (ApplicationRoles.IsDefault(role.Name!))
        {
            throw new ConflictException(string.Format(_t["Not allowed to delete {0} Role."], role.Name));
        }

        if ((await _userManager.GetUsersInRoleAsync(role.Name!)).Count > 0)
        {
            throw new ConflictException(string.Format(_t["Not allowed to delete {0} Role as it is being used."], role.Name));
        }

        await _roleManager.DeleteAsync(role);

        await _events.PublishAsync(new ApplicationRoleDeletedEvent(role.Id, role.Name!));

        return string.Format(_t["Role {0} Deleted."], role.Name);
    }
}