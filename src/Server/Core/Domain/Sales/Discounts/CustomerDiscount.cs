using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Sales.Orders;

namespace MultiMart.Domain.Sales.Discounts;

public class CustomerDiscount : AuditableEntity, IAggregateRoot
{
    public int Quantity { get; set; }
    public string? CustomerId { get; set; }
    public List<OrderDiscount> Order { get; set; } = new();
}