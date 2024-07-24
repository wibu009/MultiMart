using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Product.Book;

public class Series : AuditableEntity, IAggregateRoot
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Book> Books { get; set; } = new();
}