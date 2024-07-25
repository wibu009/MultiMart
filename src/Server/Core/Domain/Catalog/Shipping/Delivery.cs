using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Shipping;

public class Delivery : AuditableEntity, IAggregateRoot
{
    public string TrackingNumber { get; set; } = default!;
    public DateTime? EstimatedDelivery { get; set; }
    public DateTime? ActualDelivery { get; set; }
    public string AddressLine1 { get; set; } = default!;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string ContactPhone { get; set; } = default!;
    public string? ContactEmail { get; set; }
    public DefaultIdType OrderId { get; set; }
    public Order.Order Order { get; set; } = default!;
}