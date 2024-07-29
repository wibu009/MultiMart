using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Characteristics.Book;

public class BookGenre : AuditableEntity, IAggregateRoot
{
    public DefaultIdType? BookId { get; set; }
    public Products.Book Book { get; set; } = default!;
    public DefaultIdType? GenreId { get; set; }
    public Genre Genre { get; set; } = default!;
}