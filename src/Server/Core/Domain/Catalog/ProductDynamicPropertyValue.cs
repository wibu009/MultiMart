using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class ProductDynamicPropertyValue : DynamicPropertyValueEntity
{
    public DefaultIdType ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual ProductDynamicProperty DynamicProperty { get; set; } = null!;

    public ProductDynamicPropertyValue()
    {
        // Only needed for working with dapper (See GetProductDynamicPropertyValueViaDapperRequest)
        // If you're not using dapper it's better to remove this constructor.
    }

    public ProductDynamicPropertyValue(DefaultIdType dynamicPropertyId, string value, DefaultIdType productId)
        : base(dynamicPropertyId, value)
    {
        ProductId = productId;
    }
}