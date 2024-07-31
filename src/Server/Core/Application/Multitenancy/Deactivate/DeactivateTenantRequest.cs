namespace MultiMart.Application.Multitenancy.Deactivate;

public class DeactivateTenantRequest : IRequest<string>
{
    public string TenantId { get; set; } = default!;

    public DeactivateTenantRequest(string tenantId) => TenantId = tenantId;
}