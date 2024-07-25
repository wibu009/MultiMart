using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Domain.Catalog.Discount;

public class UserDiscount : AuditableEntity, IAggregateRoot
{
    public int Quantity { get; set; }
    public string? Code { get; set; }
    public decimal Value { get; set; }
    public DiscountType Type { get; set; }
    public string? UserId { get; set; }
    public string? DiscountId { get; set; }
    public Discount Discount { get; set; } = default!;
}