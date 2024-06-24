using MultiMart.Application.Multitenancy;
using MultiMart.Shared.Authorization;

namespace MultiMart.Host.Controllers.Multitenancy;

public class TenantsController : VersionNeutralApiController
{
    [HttpGet]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Tenants)]
    [SwaggerOperation("Get a list of all tenants.", "")]
    public Task<List<TenantDto>> GetListAsync()
    {
        return Mediator.Send(new GetAllTenantsRequest());
    }

    [HttpGet("{id}")]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Tenants)]
    [SwaggerOperation("Get tenant details.", "")]
    public Task<TenantDto> GetAsync(string id)
    {
        return Mediator.Send(new GetTenantRequest(id));
    }

    [HttpPost]
    [RequiresPermission(ApplicationAction.Create, ApplicationResource.Tenants)]
    [SwaggerOperation("Create a new tenant.", "")]
    public Task<string> CreateAsync(CreateTenantRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("{id}/activate")]
    [RequiresPermission(ApplicationAction.Update, ApplicationResource.Tenants)]
    [SwaggerOperation("Activate a tenant.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Register))]
    public Task<string> ActivateAsync(string id)
    {
        return Mediator.Send(new ActivateTenantRequest(id));
    }

    [HttpPost("{id}/deactivate")]
    [RequiresPermission(ApplicationAction.Update, ApplicationResource.Tenants)]
    [SwaggerOperation("Deactivate a tenant.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Register))]
    public Task<string> DeactivateAsync(string id)
    {
        return Mediator.Send(new DeactivateTenantRequest(id));
    }

    [HttpPost("{id}/upgrade")]
    [RequiresPermission(ApplicationAction.UpgradeSubscription, ApplicationResource.Tenants)]
    [SwaggerOperation("Upgrade a tenant's subscription.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Register))]
    public async Task<ActionResult<string>> UpgradeSubscriptionAsync(string id, UpgradeSubscriptionRequest request)
    {
        return id != request.TenantId
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }
}