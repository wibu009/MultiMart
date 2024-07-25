using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Shipping;

public class ShippingCompany : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string NumberPhone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public List<ShippingRate> ShippingRates { get; set; } = new();
}