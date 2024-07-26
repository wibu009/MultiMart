using MultiMart.Domain.Catalog.Orders;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Deliveries;

public class Delivery : AuditableEntity, IAggregateRoot
{
    public string TrackingNumber { get; set; } = default!;
    public DateTime? ExpectedDelivery { get; set; }
    public DateTime? ActualDelivery { get; set; }
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
    public DefaultIdType OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public DefaultIdType? ShippingRateId { get; set; }
    public ShippingRate ShippingRate { get; set; } = default!;
}