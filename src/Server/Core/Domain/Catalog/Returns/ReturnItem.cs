using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Returns;

public class ReturnItem : AuditableEntity, IAggregateRoot
{
    public int Quantity { get; set; }
    public decimal? RefundAmount { get; set; }
    public string? Note { get; set; }
    public DefaultIdType? ReturnId { get; set; }
    public Return Return { get; set; } = default!;
}