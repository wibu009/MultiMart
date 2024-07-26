using MultiMart.Domain.Catalog.Characteristic.Book;

namespace MultiMart.Domain.Catalog.Products;

public class Book : Product
{
    public string? Isbn { get; set; }
    public DateTime? PublishedOn { get; set; }
    public string? Summary { get; set; }
    public string? Language { get; set; }
    public int? TotalPages { get; set; }
    public string? Format { get; set; }
    public DefaultIdType? AuthorId { get; set; }
    public Author Author { get; set; } = default!;
    public DefaultIdType? SeriesId { get; set; }
    public Series Series { get; set; } = default!;
    public List<Genre> Genres { get; set; } = new();
}