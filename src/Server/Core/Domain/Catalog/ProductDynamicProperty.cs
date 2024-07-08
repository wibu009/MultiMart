using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class ProductDynamicProperty : DynamicPropertyEntity
{
    public DefaultIdType ProductTypeId { get; set; }
    public virtual ProductType ProductType { get; set; } = null!;
    public virtual ICollection<ProductDynamicPropertyValue> Values { get; init; } = new List<ProductDynamicPropertyValue>();

    public ProductDynamicProperty()
    {
        // Only needed for working with dapper (See GetProductDynamicPropertyViaDapperRequest)
        // If you're not using dapper it's better to remove this constructor.
    }

    public ProductDynamicProperty(DefaultIdType productTypeId, string name, string type, string description, bool isRequired)
        : base(name, type, description, isRequired)
    {
        ProductTypeId = productTypeId;
    }
}