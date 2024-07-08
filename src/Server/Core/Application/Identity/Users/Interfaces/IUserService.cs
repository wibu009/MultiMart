using System.Security.Claims;
using MultiMart.Application.Common.FileStorage;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Identity.Users.Models;
using MultiMart.Application.Identity.Users.Requests.Commands;

namespace MultiMart.Application.Identity.Users.Interfaces;

public interface IUserService : ITransientService
{
    Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);

    Task<bool> ExistsWithNameAsync(string name);
    Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);
    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);
    Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken);
    Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken);
    Task<string> AssignRolesAsync(string userId, List<UserRoleDto> userRoles, CancellationToken cancellationToken);
    Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken);
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);
    Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken);
    Task ToggleStatusAsync(bool activateUser, string userId, CancellationToken cancellationToken);
    Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
    Task<string> CreateAsync(string firstName, string lastName, string email, string userName, string password, string? phoneNumber, string? origin, CancellationToken cancellationToken);
    Task UpdateAsync(string id, string? firstName, string? lastName, string? phoneNumber, string? email, FileUploadRequest? image, bool deleteCurrentImage, CancellationToken cancellationToken);
    Task<string> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken);
    Task<string> ConfirmPhoneNumberAsync(string userId, string token);
    Task<string> ForgotPasswordAsync(string email, string? origin, CancellationToken cancellationToken = default);
    Task<string> ResetPasswordAsync(string userId, string? password, string? token);
    Task ChangePasswordAsync(string password, string newPassword, string userId);
}