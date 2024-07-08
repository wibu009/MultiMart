using MultiMart.Application.Multitenancy.Interfaces;

namespace MultiMart.Application.Multitenancy.Requests.Commands;

public class DeactivateTenantRequest : IRequest<string>
{
    public string TenantId { get; set; } = default!;

    public DeactivateTenantRequest(string tenantId) => TenantId = tenantId;
}

public class DeactivateTenantRequestHandler : IRequestHandler<DeactivateTenantRequest, string>
{
    private readonly ITenantService _tenantService;

    public DeactivateTenantRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public Task<string> Handle(DeactivateTenantRequest request, CancellationToken cancellationToken) =>
        _tenantService.DeactivateAsync(request.TenantId);
}