using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Deliveries;

public class DeliveryCompany : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string NumberPhone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public List<DeliveryRate> DeliveryRates { get; set; } = new();
}