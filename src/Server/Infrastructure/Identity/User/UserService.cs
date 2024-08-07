using System.Security.Claims;
using System.Text;
using Ardalis.Specification.EntityFrameworkCore;
using Finbuckle.MultiTenant;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using MultiMart.Application.Common.Caching;
using MultiMart.Application.Common.Events;
using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.FileStorage.LocalStorage;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Common.Mailing;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Common.Specification;
using MultiMart.Application.Identity.Users;
using MultiMart.Application.Identity.Users.Create;
using MultiMart.Application.Identity.Users.Search;
using MultiMart.Application.Identity.Users.Update;
using MultiMart.Domain.Common.Enums;
using MultiMart.Domain.Identity;
using MultiMart.Infrastructure.Auth;
using MultiMart.Infrastructure.Common;
using MultiMart.Infrastructure.Identity.Role;
using MultiMart.Infrastructure.Persistence.Context;
using MultiMart.Shared.Authorization;
using MultiMart.Shared.Multitenancy;

namespace MultiMart.Infrastructure.Identity.User;

internal partial class UserService : IUserService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;
    private readonly IJobService _jobService;
    private readonly ISendGridMailService _mailService;
    private readonly SecuritySettings _securitySettings;
    private readonly IEmailTemplateService _templateService;
    private readonly ILocalFileStorageService _localFileStorage;
    private readonly IEventPublisher _events;
    private readonly ICacheService _cache;
    private readonly ICacheKeyService _cacheKeys;
    private readonly ITenantInfo _currentTenant;

    public UserService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationDbContext db,
        IStringLocalizer<UserService> t,
        IJobService jobService,
        ISendGridMailService mailService,
        IEmailTemplateService templateService,
        ILocalFileStorageService localFileStorage,
        IEventPublisher events,
        ICacheService cache,
        ICacheKeyService cacheKeys,
        ITenantInfo currentTenant,
        IOptions<SecuritySettings> securitySettings)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
        _t = t;
        _jobService = jobService;
        _mailService = mailService;
        _templateService = templateService;
        _localFileStorage = localFileStorage;
        _events = events;
        _cache = cache;
        _cacheKeys = cacheKeys;
        _currentTenant = currentTenant;
        _securitySettings = securitySettings.Value;
    }

    #region Base Queries

    public async Task<PaginationResponse<UserDetailsDto>> SearchAsync(SearchUserRequest request, CancellationToken cancellationToken)
    {
        var pagingSpec = new EntitiesByPaginationFilterSpec<ApplicationUser>(request);

        var users = await _userManager.Users
            .WithSpecification(pagingSpec)
            .ProjectToType<UserDetailsDto>()
            .ToListAsync(cancellationToken);

        var filterSpec = new EntitiesByBaseFilterSpec<ApplicationUser>(request);
        int count = await _userManager.Users
            .WithSpecification(filterSpec)
            .CountAsync(cancellationToken);

        return new PaginationResponse<UserDetailsDto>(users, count, request.PageNumber, request.PageSize);
    }

    public async Task<PaginationResponse<TUserDto>> SearchAsync<TUserDto, TSearchUserRequest>(
        TSearchUserRequest request,
        CancellationToken cancellationToken)
        where TUserDto : UserDetailsDto
        where TSearchUserRequest : PaginationFilter
    {
        var query = _userManager.Users;

        if (typeof(TUserDto) == typeof(CustomerDetailsDto))
        {
            query = query.OfType<Customer>();
        }
        else if (typeof(TUserDto) == typeof(EmployeeDetailsDto))
        {
            query = query.OfType<Employee>();
        }

        var pagingSpec = new EntitiesByPaginationFilterSpec<ApplicationUser>(request);

        var users = await query
            .WithSpecification(pagingSpec)
            .ProjectToType<TUserDto>()
            .ToListAsync(cancellationToken);

        var filterSpec = new EntitiesByBaseFilterSpec<ApplicationUser>(request);
        int count = await query
            .WithSpecification(filterSpec)
            .CountAsync(cancellationToken);

        return new PaginationResponse<TUserDto>(users, count, request.PageNumber, request.PageSize);
    }

    public async Task<bool> ExistsWithNameAsync(string name)
    {
        EnsureValidTenant();
        return await _userManager.FindByNameAsync(name) is not null;
    }

    public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
    {
        EnsureValidTenant();
        return await _userManager.FindByEmailAsync(email.Normalize()) is { } user && user.Id != exceptId;
    }

    public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
    {
        EnsureValidTenant();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is { } user && user.Id != exceptId;
    }

    private void EnsureValidTenant()
    {
        if (string.IsNullOrWhiteSpace(_currentTenant.Id))
        {
            throw new UnauthorizedException(_t["Invalid Tenant."]);
        }
    }

    public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken) =>
        (await _userManager.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken))
            .Adapt<List<UserDetailsDto>>();

    public async Task<List<TUserDto>> GetListAsync<TUserDto>(CancellationToken cancellationToken)
        where TUserDto : UserDetailsDto
    {
        var query = _userManager.Users;

        if (typeof(TUserDto) == typeof(CustomerDetailsDto))
        {
            query = query.OfType<Customer>();
        }
        else if (typeof(TUserDto) == typeof(EmployeeDetailsDto))
        {
            query = query.OfType<Employee>();
        }

        return (await query
                .AsNoTracking()
                .ToListAsync(cancellationToken))
            .Adapt<List<TUserDto>>();
    }

    public Task<int> CountAsync(CancellationToken cancellationToken) =>
        _userManager.Users.AsNoTracking().CountAsync(cancellationToken);

    public async Task<int> CountAsync<TUserDto>(CancellationToken cancellationToken)
        where TUserDto : UserDetailsDto
    {
        var query = _userManager.Users;

        if (typeof(TUserDto) == typeof(CustomerDetailsDto))
        {
            query = query.OfType<Customer>();
        }
        else if (typeof(TUserDto) == typeof(EmployeeDetailsDto))
        {
            query = query.OfType<Employee>();
        }

        return await query.AsNoTracking().CountAsync(cancellationToken);
    }

    public async Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        return user.Adapt<UserDetailsDto>();
    }

    public async Task<TUserDto> GetAsync<TUserDto>(string userId, CancellationToken cancellationToken)
        where TUserDto : UserDetailsDto
    {
        var query = _userManager.Users;

        if (typeof(TUserDto) == typeof(CustomerDetailsDto))
        {
            query = query.OfType<Customer>();
        }
        else if (typeof(TUserDto) == typeof(EmployeeDetailsDto))
        {
            query = query.OfType<Employee>();
        }

        var user = await query
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        return user.Adapt<TUserDto>();
    }

    public async Task ToggleStatusAsync(bool activateUser, string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        bool isAdmin = await _userManager.IsInRoleAsync(user, ApplicationRoles.Admin);
        if (isAdmin)
        {
            throw new ConflictException(_t["Administrators Profile's Status cannot be toggled"]);
        }

        user.IsActive = activateUser;

        await _userManager.UpdateAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
    }

    #endregion

    #region Confirm Email/Phone

    private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
    {
        EnsureValidTenant();

        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "api/users/confirm-email/";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), MultitenancyConstants.TenantIdName, _currentTenant.Id!);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.UserId, user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
        return verificationUri;
    }

    public async Task<string> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken)
    {
        EnsureValidTenant();

        var user = await _userManager.Users
            .Where(u => u.Id == userId && !u.EmailConfirmed)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new InternalServerException(_t["An error occurred while confirming E-Mail."]);

        token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded
            ? string.Format(_t["Account Confirmed for E-Mail {0}. You can now use the /api/tokens endpoint to generate JWT."], user.Email)
            : throw new InternalServerException(string.Format(_t["An error occurred while confirming {0}"], user.Email));
    }

    public async Task<string> ConfirmPhoneNumberAsync(string userId, string token)
    {
        EnsureValidTenant();

        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new InternalServerException(_t["An error occurred while confirming Mobile Phone."]);
        if (string.IsNullOrEmpty(user.PhoneNumber)) throw new InternalServerException(_t["An error occurred while confirming Mobile Phone."]);

        var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, token);

        return result.Succeeded
            ? user.PhoneNumberConfirmed
                ? string.Format(_t["Account Confirmed for Phone Number {0}. You can now use the /api/tokens endpoint to generate JWT."], user.PhoneNumber)
                : string.Format(_t["Account Confirmed for Phone Number {0}. You should confirm your E-mail before using the /api/tokens endpoint to generate JWT."], user.PhoneNumber)
            : throw new InternalServerException(string.Format(_t["An error occurred while confirming {0}"], user.PhoneNumber));
    }

    #endregion

    #region Create/Update

    /// <summary>
    /// This is used when authenticating with AzureAd.
    /// The local user is retrieved using the objectidentifier claim present in the ClaimsPrincipal.
    /// If no such claim is found, an InternalServerException is thrown.
    /// If no user is found with that ObjectId, a new one is created and populated with the values from the ClaimsPrincipal.
    /// If a role claim is present in the principal, and the user is not yet in that roll, then the user is added to that role.
    /// </summary>
    public async Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        string? objectId = principal.GetObjectId();
        if (string.IsNullOrWhiteSpace(objectId))
        {
            throw new InternalServerException(_t["Invalid objectId"]);
        }

        var user = await _userManager.Users.Where(u => u.ObjectId == objectId).FirstOrDefaultAsync()
            ?? await CreateOrUpdateFromPrincipalAsync(principal);

        if (principal.FindFirstValue(ClaimTypes.Role) is { } role &&
            await _roleManager.RoleExistsAsync(role) &&
            !await _userManager.IsInRoleAsync(user, role))
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        return user.Id;
    }

    private async Task<ApplicationUser> CreateOrUpdateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        string? email = principal.FindFirstValue(ClaimTypes.Upn);
        string? username = principal.GetDisplayName();
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
        {
            throw new InternalServerException(string.Format(_t["Username or Email not valid."]));
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
        {
            throw new InternalServerException(string.Format(_t["Username {0} is already taken."], username));
        }

        if (user is null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
            {
                throw new InternalServerException(string.Format(_t["Email {0} is already taken."], email));
            }
        }

        IdentityResult? result;
        if (user is not null)
        {
            user.ObjectId = principal.GetObjectId();
            result = await _userManager.UpdateAsync(user);

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
        }
        else
        {
            user = new ApplicationUser
            {
                ObjectId = principal.GetObjectId(),
                FirstName = principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = principal.FindFirstValue(ClaimTypes.Surname),
                Gender = Gender.NotKnown,
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                UserName = username,
                NormalizedUserName = username.ToUpperInvariant(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            result = await _userManager.CreateAsync(user);

            await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));
        }

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Validation Errors Occurred."], result.GetErrors(_t));
        }

        return user;
    }

    public async Task<string> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = request.Adapt<ApplicationUser>();

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Validation Errors Occurred."], result.GetErrors(_t));
        }

        await _userManager.AddToRoleAsync(user, ApplicationRoles.Basic);

        var messages = new List<string> { string.Format(_t["User {0} Registered."], user.UserName) };

        if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
        {
            // send verification email
            string emailVerificationUri = await GetEmailVerificationUriAsync(user, request.Origin!);
            var emailModel = new UserEmailTemplateModel
            {
                Email = user.Email,
                UserName = user.UserName!,
                Url = emailVerificationUri
            };
            var mailRequest = new MailRequest(
                new List<string> { user.Email },
                _t["Confirm Registration"],
                _templateService.GenerateEmailTemplate("email-confirmation", emailModel));
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, cancellationToken));
            messages.Add(_t[$"Please check {user.Email} to verify your account!"]);
        }

        await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));

        return string.Join(Environment.NewLine, messages);
    }

    public async Task<string> CreateAsync<TCreateUserRequest>(
        TCreateUserRequest request,
        CancellationToken cancellationToken)
        where TCreateUserRequest : CreateUserRequest
    {
        dynamic user = request switch
        {
            CreateCustomerRequest => request.Adapt<Customer>(),
            CreateEmployeeRequest => request.Adapt<Employee>(),
            _ => request.Adapt<ApplicationUser>()
        };

        dynamic result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Validation Errors Occurred."], result.GetErrors(_t));
        }

        string role = user switch
        {
            Customer => ApplicationRoles.Basic,
            Employee => ApplicationRoles.Admin,
            _ => ApplicationRoles.Basic // Default role, if any
        };

        await _userManager.AddToRoleAsync(user, role);

        var messages = new List<string> { string.Format(_t["User {0} Registered."], user.UserName) };

        if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
        {
            string emailVerificationUri = await GetEmailVerificationUriAsync(user, request.Origin!);
            var emailModel = new UserEmailTemplateModel
            {
                Email = user.Email,
                UserName = user.UserName!,
                Url = emailVerificationUri
            };
            var mailRequest = new MailRequest(
                new List<string> { user.Email },
                _t["Confirm Registration"],
                _templateService.GenerateEmailTemplate("email-confirmation", emailModel));
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, cancellationToken));
            messages.Add(_t[$"Please check {user.Email} to verify your account!"]);
        }

        await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));

        return string.Join(Environment.NewLine, messages);
    }

    public async Task UpdateAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(request.Id);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        string currentImage = user.Avatar ?? string.Empty;
        if (request.Image != null || request.DeleteCurrentImage)
        {
            user.Avatar = await _localFileStorage.UploadAsync<ApplicationUser>(request.Image, FileType.Image, cancellationToken);
            if (request.DeleteCurrentImage && !string.IsNullOrEmpty(currentImage))
            {
                string root = Directory.GetCurrentDirectory();
                await _localFileStorage.RemoveAsync(Path.Combine(root, currentImage), cancellationToken);
            }
        }

        user = request.Adapt(user);

        string? currentPhoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (request.PhoneNumber != currentPhoneNumber)
        {
            await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        }

        string? currentEmail = await _userManager.GetEmailAsync(user);
        if (request.Email != currentEmail)
        {
            await _userManager.SetEmailAsync(user, request.Email);
        }

        var result = await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Update profile failed"], result.GetErrors(_t));
        }
    }

    public async Task UpdateAsync<TUpdateUserRequest>(
        TUpdateUserRequest request,
        CancellationToken cancellationToken = default)
        where TUpdateUserRequest : UpdateUserRequest
    {
        var query = _userManager.Users;

        if (typeof(TUpdateUserRequest) == typeof(UpdateCustomerRequest))
        {
            query = query.OfType<Customer>();
        }
        else if (typeof(TUpdateUserRequest) == typeof(UpdateEmployeeRequest))
        {
            query = query.OfType<Employee>();
        }

        var user = await query
            .Where(u => u.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        string currentImage = user.Avatar ?? string.Empty;
        if (request.Image != null || request.DeleteCurrentImage)
        {
            user.Avatar = await _localFileStorage.UploadAsync<ApplicationUser>(request.Image, FileType.Image, cancellationToken);
            if (request.DeleteCurrentImage && !string.IsNullOrEmpty(currentImage))
            {
                string root = Directory.GetCurrentDirectory();
                await _localFileStorage.RemoveAsync(Path.Combine(root, currentImage), cancellationToken);
            }
        }

        user = request.Adapt(user);

        string? currentPhoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (request.PhoneNumber != currentPhoneNumber)
        {
            await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        }

        string? currentEmail = await _userManager.GetEmailAsync(user);
        if (request.Email != currentEmail)
        {
            await _userManager.SetEmailAsync(user, request.Email);
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Update profile failed"], result.GetErrors(_t));
        }

        await _signInManager.RefreshSignInAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
    }

    #endregion

    #region Password

    public async Task<string> ForgotPasswordAsync(string email, string? origin, CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        // Find by email or username
        var user = await _userManager.FindByEmailAsync(email.Normalize()!);
        if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            // Don't reveal that the user does not exist or is not confirmed
            throw new InternalServerException(_t["An Error has occurred!"]);
        }

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        string passwordResetUri = await GetEmailForgotPasswordUriAsync(user, origin!);
        var emailModel = new UserEmailTemplateModel()
        {
            Email = user.Email!,
            UserName = user.UserName!,
            Url = passwordResetUri
        };
        var mailRequest = new MailRequest(
            new List<string> { email },
            _t["Reset Password"],
            _templateService.GenerateEmailTemplate("email-reset-password", emailModel));
        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, cancellationToken));

        return _t["Password Reset Mail has been sent to your authorized Email."];
    }

    public async Task<string> ResetPasswordAsync(string userId, string? password, string? token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        // Don't reveal that the user does not exist
        _ = user ?? throw new InternalServerException(_t["An Error has occurred!"]);

        string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token!));
        var result = await _userManager.ResetPasswordAsync(user, code, password!);

        return result.Succeeded
            ? _t["Password Reset Successful!"]
            : throw new InternalServerException(_t["An Error has occurred!"]);
    }

    public async Task ChangePasswordAsync(string password, string newPassword, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        var result = await _userManager.ChangePasswordAsync(user, password, newPassword);

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Change password failed"], result.GetErrors(_t));
        }
    }

    private async Task<string> GetEmailForgotPasswordUriAsync(ApplicationUser user, string origin)
    {
        EnsureValidTenant();

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        const string route = "account/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        string passwordResetUri = QueryHelpers.AddQueryString(endpointUri.ToString(), MultitenancyConstants.TenantIdName, _currentTenant.Id!);
        passwordResetUri = QueryHelpers.AddQueryString(passwordResetUri, QueryStringKeys.UserId, user.Id);
        passwordResetUri = QueryHelpers.AddQueryString(passwordResetUri, QueryStringKeys.Token, token);
        return passwordResetUri;
    }

    #endregion

    #region Permissions

    public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new UnauthorizedException("Authentication Failed.");

        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        foreach (var role in await _roleManager.Roles
                     .Where(r => userRoles.Contains(r.Name!))
                     .ToListAsync(cancellationToken))
        {
            permissions.AddRange(await _db.RoleClaims
                .Where(rc => rc.RoleId == role.Id && rc.ClaimType == ApplicationClaims.Permission)
                .Select(rc => rc.ClaimValue!)
                .ToListAsync(cancellationToken));
        }

        return permissions.Distinct().ToList();
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken)
    {
        var permissions = await _cache.GetOrSetAsync(
            _cacheKeys.GetCacheKey(ApplicationClaims.Permission, userId),
            () => GetPermissionsAsync(userId, cancellationToken),
            cancellationToken: cancellationToken);

        return permissions?.Contains(permission) ?? false;
    }

    public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken) =>
        _cache.RemoveAsync(_cacheKeys.GetCacheKey(ApplicationClaims.Permission, userId), cancellationToken);

    #endregion

    #region Roles

    public async Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var userRoles = new List<UserRoleDto>();

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new NotFoundException("User Not Found.");
        var roles = await _roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken);
        if (roles is null) throw new NotFoundException("Roles Not Found.");
        foreach (var role in roles)
        {
            userRoles.Add(new UserRoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                Enabled = await _userManager.IsInRoleAsync(user, role.Name!)
            });
        }

        return userRoles;
    }

    public async Task<string> AssignRolesAsync(string userId, List<UserRoleDto> userRoles, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userRoles, nameof(userRoles));

        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        // Check if the user is an admin for which the admin role is getting disabled
        if (await _userManager.IsInRoleAsync(user, ApplicationRoles.Admin)
            && userRoles.Any(a => !a.Enabled && a.RoleName == ApplicationRoles.Admin))
        {
            // Get count of users in Admin Role
            int adminCount = (await _userManager.GetUsersInRoleAsync(ApplicationRoles.Admin)).Count;

            // Check if user is not Root Tenant Admin
            if (user.Email == MultitenancyConstants.Root.EmailAddress)
            {
                if (_currentTenant.Id == MultitenancyConstants.Root.Id)
                {
                    throw new ConflictException(_t["Cannot Remove Admin Role From Root Tenant Admin."]);
                }
            }
            else if (adminCount <= 2)
            {
                throw new ConflictException(_t["Tenant should have at least 2 Admins."]);
            }
        }

        foreach (var userRole in userRoles)
        {
            // Check if Role Exists
            if (await _roleManager.FindByNameAsync(userRole.RoleName!) is not null)
            {
                if (userRole.Enabled)
                {
                    if (!await _userManager.IsInRoleAsync(user, userRole.RoleName!))
                    {
                        await _userManager.AddToRoleAsync(user, userRole.RoleName!);
                    }
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole.RoleName!);
                }
            }
        }

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id, true));

        return _t["User Roles Updated Successfully."];
    }

    #endregion
}