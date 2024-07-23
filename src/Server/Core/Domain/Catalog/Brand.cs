using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class Brand : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public List<Product.Product> Products { get; set; } = new();
}