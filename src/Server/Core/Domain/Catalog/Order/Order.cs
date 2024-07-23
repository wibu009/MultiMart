using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Domain.Catalog.Order;

public class Order : AuditableEntity, IAggregateRoot
{
    public OrderType Type { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? Note { get; set; }
    public string? CustomerId { get; set; }
    public DefaultIdType? ReturnId { get; set; }
    public Return.Return? Return { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
}