namespace MultiMart.Application.Multitenancy.Get;

public class GetTenantRequest : IRequest<TenantDto>
{
    public string TenantId { get; set; } = default!;

    public GetTenantRequest(string tenantId) => TenantId = tenantId;
}