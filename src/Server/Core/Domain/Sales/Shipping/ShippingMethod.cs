using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Sales.Orders;

namespace MultiMart.Domain.Sales.Shipping;

public class ShippingMethod : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal? Cost { get; set; }
    public string? Currency { get; set; }
    public int? EstimatedDeliveryDays { get; set; }
    public List<Order> Orders { get; set; } = new();
}