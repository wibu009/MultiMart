﻿using MultiMart.Application.Multitenancy.Interfaces;
using MultiMart.Application.Multitenancy.Models;

namespace MultiMart.Application.Multitenancy.Requests.Queries;

public class GetAllTenantsRequest : IRequest<List<TenantDto>>
{
}

public class GetAllTenantsRequestHandler : IRequestHandler<GetAllTenantsRequest, List<TenantDto>>
{
    private readonly ITenantService _tenantService;

    public GetAllTenantsRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public Task<List<TenantDto>> Handle(GetAllTenantsRequest request, CancellationToken cancellationToken) =>
        _tenantService.GetAllAsync();
}