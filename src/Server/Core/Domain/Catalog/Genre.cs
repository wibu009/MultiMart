using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class Genre : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}