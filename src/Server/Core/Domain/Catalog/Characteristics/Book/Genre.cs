using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Characteristics.Book;

public class Genre : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<BookGenre> Books { get; set; } = new();
}