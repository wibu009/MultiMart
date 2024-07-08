using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class ProductType : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public ICollection<Product> Products { get; init; } = new List<Product>();
    public ICollection<ProductDynamicProperty> ProductDynamicProperties { get; init; } = new List<ProductDynamicProperty>();

    public ProductType()
    {
        // Only needed for working with dapper (See GetProductTypeViaDapperRequest)
        // If you're not using dapper it's better to remove this constructor.
    }

    public ProductType(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public ProductType Update(string? name, string? description)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        return this;
    }
}