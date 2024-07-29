using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Sales.Orders;

namespace MultiMart.Domain.Sales.Deliveries;

public class Delivery : AuditableEntity, IAggregateRoot
{
    public string TrackingNumber { get; set; } = default!;
    public DateTime ShippingDate { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    public decimal ShippingCost { get; set; }
    public string AddressLine1 { get; set; } = default!;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string ContactPhone { get; set; } = default!;
    public string? ContactEmail { get; set; }
    public string? Notes { get; set; }
    public DefaultIdType? OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public DefaultIdType? DeliveryRateId { get; set; }
    public DeliveryRate DeliveryRate { get; set; } = default!;
}