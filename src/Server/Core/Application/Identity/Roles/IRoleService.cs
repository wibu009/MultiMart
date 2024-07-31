using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Identity.Roles;

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