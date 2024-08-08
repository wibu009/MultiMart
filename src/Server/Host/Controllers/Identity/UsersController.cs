using MultiMart.Application.Common.Models;
using MultiMart.Application.Identity.Users;
using MultiMart.Application.Identity.Users.ConfirmEmail;
using MultiMart.Application.Identity.Users.ConfirmPhoneNumber;
using MultiMart.Application.Identity.Users.Create;
using MultiMart.Application.Identity.Users.Create.Customer;
using MultiMart.Application.Identity.Users.Create.Employee;
using MultiMart.Application.Identity.Users.ForgotPassword;
using MultiMart.Application.Identity.Users.Get;
using MultiMart.Application.Identity.Users.GetUserRole;
using MultiMart.Application.Identity.Users.ResetPassword;
using MultiMart.Application.Identity.Users.Search;
using MultiMart.Application.Identity.Users.SetUserRole;
using MultiMart.Application.Identity.Users.ToggleUserStatus;
using MultiMart.Infrastructure.Common.Extensions;
using MultiMart.Infrastructure.OpenApi;

namespace MultiMart.Host.Controllers.Identity;

public class UsersController : VersionNeutralApiController
{
    [HttpPost("search")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Users)]
    [SwaggerOperation("Search for users.", "")]
    public async Task<PaginationResponse<UserDetailsDto>> SearchAsync(SearchUserRequest request, CancellationToken cancellationToken)
        => await Mediator.Send(request, cancellationToken);

    [HttpPost("search-customer")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Users)]
    [SwaggerOperation("Search for customers.", "")]
    public async Task<PaginationResponse<CustomerDetailsDto>> SearchCustomerAsync(SearchCustomerRequest request, CancellationToken cancellationToken)
        => await Mediator.Send(request, cancellationToken);

    [HttpGet("{id}")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Users)]
    [SwaggerOperation("Get a user's details.", "")]
    public async Task<UserDetailsDto> GetByIdAsync(string id, CancellationToken cancellationToken)
        => await Mediator.Send(new GetUserRequest(id), cancellationToken);

    [HttpGet("{id}/customer")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Users)]
    [SwaggerOperation("Get a customer's details.", "")]
    public async Task<CustomerDetailsDto> GetCustomerByIdAsync(string id, CancellationToken cancellationToken)
        => await Mediator.Send(new GetCustomerRequest(id), cancellationToken);

    [HttpGet("{id}/roles")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.UserRoles)]
    [SwaggerOperation("Get a user's roles.", "")]
    public async Task<List<UserRoleDto>> GetRolesAsync(string id, CancellationToken cancellationToken)
        => await Mediator.Send(new GetUserRoleRequest(id), cancellationToken);

    [HttpPost("{id}/roles")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Register))]
    [RequiresPermission(ApplicationAction.Update, ApplicationResource.UserRoles)]
    [SwaggerOperation("Update a user's assigned roles.", "")]
    public async Task<string> AssignRolesAsync(string id, SetUserRolesRequest request, CancellationToken cancellationToken)
        => await Mediator.Send(request.SetPropertyValue(nameof(request.UserId), id), cancellationToken);

    [HttpPost("customer")]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Users)]
    [SwaggerOperation("Create a customer.", "")]
    public async Task<string> CreateCustomerAsync(CreateCustomerRequest request)
        => await Mediator.Send(request);

    [HttpPost("employee")]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Users)]
    [SwaggerOperation("Create an employee.", "")]
    public async Task<string> CreateEmployeeAsync(CreateEmployeeRequest request)
        => await Mediator.Send(request);

    [HttpPost("self-register")]
    [TenantIdHeader]
    [AllowAnonymous]
    [SwaggerOperation("Anonymous user creates a user.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Register))]
    public async Task<string> SelfRegisterAsync(CreateUserRequest request)
        => await Mediator.Send(request.SetPropertyValue(nameof(request.Origin), GetOriginFromRequest()));

    [HttpPost("{id}/toggle-status")]
    [RequiresPermission(ApplicationAction.Update, ApplicationResource.Users)]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Register))]
    [SwaggerOperation("Toggle a user's active status.", "")]
    public async Task<ActionResult> ToggleStatusAsync(string id, ToggleUserStatusRequest request, CancellationToken cancellationToken)
        => Ok(await Mediator.Send(request.SetPropertyValue(nameof(request.UserId), id), cancellationToken));

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    [SwaggerOperation("Confirm email address for a user.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Search))]
    public async Task<string> ConfirmEmailAsync([FromQuery] string tenant, [FromQuery] string userId, [FromQuery] string token, CancellationToken cancellationToken)
        => await Mediator.Send(new ConfirmEmailRequest(token, userId), cancellationToken);

    [HttpGet("confirm-phone-number")]
    [AllowAnonymous]
    [SwaggerOperation("Confirm phone number for a user.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Search))]
    public Task<string> ConfirmPhoneNumberAsync([FromQuery] string userId, [FromQuery] string token)
        => Mediator.Send(new ConfirmPhoneNumberRequest(userId, token));

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [TenantIdHeader]
    [SwaggerOperation("Request a password reset email for a user.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Register))]
    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
        => await Mediator.Send(request.SetPropertyValue(nameof(request.Origin), GetOriginFromRequest()));

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [TenantIdHeader]
    [SwaggerOperation("Reset a user's password.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Register))]
    public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
        => await Mediator.Send(request);

    private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
}
