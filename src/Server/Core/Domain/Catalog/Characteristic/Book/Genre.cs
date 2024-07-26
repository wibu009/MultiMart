using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Characteristic.Book;

public class Genre : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<Products.Book> Books { get; set; } = new();
}