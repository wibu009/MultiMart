using MultiMart.Domain.Catalog.Addresses;
using MultiMart.Domain.Sales.Orders;

namespace MultiMart.Domain.Sales.Shipping;

public class ShippingAddress : Address
{
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public DefaultIdType? OrderId { get; set; }
    public Order Order { get; set; } = default!;
}