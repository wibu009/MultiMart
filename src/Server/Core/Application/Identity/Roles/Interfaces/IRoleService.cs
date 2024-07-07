using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Identity.Roles.Models;
using MultiMart.Application.Identity.Roles.Requests.Commands;

namespace MultiMart.Application.Identity.Roles.Interfaces;

public interface IRoleService : ITransientService
{
    Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string roleName, string? excludeId);
    Task<RoleDto> GetByIdAsync(string id);
    Task<string> CreateOrUpdateAsync(string? id, string name, string? description);
    Task<string> UpdatePermissionsAsync(string roleId, List<string> permissions);
    Task<string> DeleteAsync(string id);
}