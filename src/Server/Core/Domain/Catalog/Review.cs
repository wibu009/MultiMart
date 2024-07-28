using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class Review : AuditableEntity, IAggregateRoot
{
    public string Title { get; set; } = default!;
    public string? Content { get; set; }
    public decimal Rating { get; set; }
    public string? CustomerId { get; set; }
    public DefaultIdType? ProductId { get; set; }
    public Product Product { get; set; } = default!;
}