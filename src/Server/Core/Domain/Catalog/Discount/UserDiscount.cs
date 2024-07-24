using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Discount;

public class UserDiscount : AuditableEntity, IAggregateRoot
{
    public string? UserId { get; set; }
    public string? DiscountId { get; set; }
    public Discount Discount { get; set; } = default!;
    public int UsageCount { get; set; }
    public DateTime? LastUsage { get; set; }
    public bool IsActive { get; set; }
}