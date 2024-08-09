using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;
using MultiMart.Domain.Sales.Discounts;

namespace MultiMart.Domain.Sales.Orders;

public class OrderDiscount : AuditableEntity, IAggregateRoot
{
    public string Code { get; set; } = default!;
    public decimal Value { get; set; }
    public CalculationType Type { get; set; }
    public DefaultIdType? DiscountId { get; set; }
    public DiscountOnCustomer Discount { get; set; } = default!;
    public DefaultIdType? OrderId { get; set; }
    public Order Order { get; set; } = default!;
}