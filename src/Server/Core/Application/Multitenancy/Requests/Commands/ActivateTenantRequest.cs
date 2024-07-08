using MultiMart.Application.Multitenancy.Interfaces;

namespace MultiMart.Application.Multitenancy.Requests.Commands;

public class ActivateTenantRequest : IRequest<string>
{
    public string TenantId { get; set; } = default!;

    public ActivateTenantRequest(string tenantId) => TenantId = tenantId;
}

public class ActivateTenantRequestHandler : IRequestHandler<ActivateTenantRequest, string>
{
    private readonly ITenantService _tenantService;

    public ActivateTenantRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public Task<string> Handle(ActivateTenantRequest request, CancellationToken cancellationToken) =>
        _tenantService.ActivateAsync(request.TenantId);
}