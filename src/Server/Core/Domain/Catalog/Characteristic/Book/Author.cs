using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Characteristic.Book;

public class Author : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string? Biography { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }
    public List<Products.Book> Books { get; set; } = new();
}