using System.Security.Claims;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Identity.Users.Create;
using MultiMart.Application.Identity.Users.Search;
using MultiMart.Application.Identity.Users.Update;

namespace MultiMart.Application.Identity.Users;

public interface IUserService : IScopedService
{
    Task<PaginationResponse<UserDetailsDto>> SearchAsync(SearchUserRequest request, CancellationToken cancellationToken);

    Task<PaginationResponse<TUserDto>> SearchAsync<TUserDto, TSearchUserRequest>(TSearchUserRequest request, CancellationToken cancellationToken)
        where TUserDto : UserDetailsDto
        where TSearchUserRequest : PaginationFilter;
    Task<bool> ExistsWithNameAsync(string name);
    Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);
    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);
    Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);
    Task<List<TUserDto>> GetListAsync<TUserDto>(CancellationToken cancellationToken)
        where TUserDto : UserDetailsDto;
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<int> CountAsync<TUserDto>(CancellationToken cancellationToken)
        where TUserDto : UserDetailsDto;
    Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken);
    Task<TUserDto> GetAsync<TUserDto>(string userId, CancellationToken cancellationToken)
        where TUserDto : UserDetailsDto;
    Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken);
    Task<string> AssignRolesAsync(string userId, List<UserRoleDto> userRoles, CancellationToken cancellationToken);
    Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken);
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);
    Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken);
    Task ToggleStatusAsync(bool activateUser, string userId, CancellationToken cancellationToken);
    Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
    Task<string> CreateAsync<TCreateUserRequest>(TCreateUserRequest request, CancellationToken cancellationToken)
        where TCreateUserRequest : CreateUserRequest;
    Task UpdateAsync(UpdateUserRequest request, CancellationToken cancellationToken);
    Task UpdateAsync<TUpdateUserRequest>(TUpdateUserRequest request, CancellationToken cancellationToken = default)
        where TUpdateUserRequest : UpdateUserRequest;
    Task<string> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken);
    Task<string> ConfirmPhoneNumberAsync(string userId, string token);
    Task<string> ForgotPasswordAsync(string email, string? origin, CancellationToken cancellationToken);
    Task<string> ResetPasswordAsync(string userId, string? password, string? token);
    Task ChangePasswordAsync(string password, string newPassword, string userId);
}