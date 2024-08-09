using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;
using MultiMart.Domain.Sales.Deliveries;
using MultiMart.Domain.Sales.Returns;
using MultiMart.Domain.Sales.Shipping;

namespace MultiMart.Domain.Sales.Orders;

public class Order : AuditableEntity, IAggregateRoot
{
    public OrderType Type { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public string Code { get; set; } = default!;
    public decimal? TotalPrice { get; set; }
    public decimal? TotalDiscount { get; set; }
    public string? Note { get; set; }
    public string? CustomerId { get; set; }
    public DefaultIdType? ReturnId { get; set; }
    public Return Return { get; set; } = new();
    public DefaultIdType? ShippingMethodId { get; set; }
    public ShippingMethod ShippingMethod { get; set; } = new();
    public DefaultIdType? ShippingAddressId { get; set; }
    public ShippingAddress ShippingAddress { get; set; } = new();
    public List<OrderItem> Items { get; set; } = new();
    public List<OrderDiscount> Discounts { get; set; } = new();
}