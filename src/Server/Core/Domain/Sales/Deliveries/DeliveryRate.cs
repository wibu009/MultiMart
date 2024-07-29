using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Sales.Deliveries;

public class DeliveryRate : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string WeightUnit { get; set; } = default!;
    public string DistanceUnit { get; set; } = default!;
    public decimal WeightFrom { get; set; }
    public decimal WeightTo { get; set; }
    public decimal DistanceFrom { get; set; }
    public decimal DistanceTo { get; set; }
    public decimal PricePerWeight { get; set; }
    public decimal PricePerDistance { get; set; }
    public decimal BasePrice { get; set; }
    public string Currency { get; set; } = default!;
    public DefaultIdType? DeliveryCompanyId { get; set; }
    public DeliveryCompany DeliveryCompany { get; set; } = default!;
    public List<Delivery> Deliveries { get; set; } = new();

    public decimal Calculate(decimal packageWeight, decimal shippingDistance)
    {
        if (packageWeight < WeightFrom || packageWeight > WeightTo)
        {
            throw new ArgumentException($"Package weight {packageWeight} is out of the allowed range {WeightFrom} - {WeightTo} {WeightUnit}.");
        }

        if (shippingDistance < DistanceFrom || shippingDistance > DistanceTo)
        {
            throw new ArgumentException($"Shipping distance {shippingDistance} is out of the allowed range {DistanceFrom} - {DistanceTo} {DistanceUnit}.");
        }

        decimal weightPrice = packageWeight * PricePerWeight;
        decimal distancePrice = shippingDistance * PricePerDistance;
        decimal totalPrice = BasePrice + weightPrice + distancePrice;

        return totalPrice;
    }
}