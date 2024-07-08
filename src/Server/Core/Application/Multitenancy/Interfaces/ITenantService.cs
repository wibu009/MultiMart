using MultiMart.Application.Multitenancy.Models;
using MultiMart.Application.Multitenancy.Requests.Commands;

namespace MultiMart.Application.Multitenancy.Interfaces;

public interface ITenantService
{
    Task<List<TenantDto>> GetAllAsync();
    Task<bool> ExistsWithIdAsync(string id);
    Task<bool> ExistsWithNameAsync(string name);
    Task<TenantDto> GetByIdAsync(string id);
    Task<string> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken);
    Task<string> ActivateAsync(string id);
    Task<string> DeactivateAsync(string id);
    Task<string> UpdateSubscription(string id, DateTime extendedExpiryDate);
}