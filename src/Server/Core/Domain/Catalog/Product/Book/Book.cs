namespace MultiMart.Domain.Catalog.Product.Book;

public class Book : Product
{
    public string? Isbn { get; set; }
    public DateTime? PublishedOn { get; set; }
    public string? Summary { get; set; }
    public string? Language { get; set; }
    public int? TotalPages { get; set; }
    public decimal? Weight { get; set; }
    public string? Dimensions { get; set; }
    public string? Format { get; set; }
    public DefaultIdType? AuthorId { get; set; }
    public Author Author { get; set; } = default!;
    public List<Genre> Genres { get; set; } = new();
}