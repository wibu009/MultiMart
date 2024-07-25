using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Shipping;

public class ShippingRate : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public decimal WeightFrom { get; set; }
    public decimal WeightTo { get; set; }
    public decimal Distance { get; set; }
    public decimal Rate { get; set; }
    public DefaultIdType ShippingCompanyId { get; set; }
    public ShippingCompany ShippingCompany { get; set; } = default!;
}