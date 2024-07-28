using MultiMart.Domain.Catalog.Discounts;
using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Domain.Catalog.Orders;

public class OrderDiscount : AuditableEntity, IAggregateRoot
{
    public string Code { get; set; } = default!;
    public decimal Value { get; set; }
    public DiscountType Type { get; set; }
    public DefaultIdType? DiscountId { get; set; }
    public CustomerDiscount Discount { get; set; } = default!;
    public DefaultIdType? OrderId { get; set; }
    public Orders.Order Order { get; set; } = default!;
}