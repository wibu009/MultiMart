using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Product.Book;

public class Genre : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<Book> Books { get; set; } = new();
}