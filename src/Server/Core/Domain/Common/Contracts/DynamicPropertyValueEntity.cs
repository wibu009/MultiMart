namespace MultiMart.Domain.Common.Contracts;

public abstract class DynamicPropertyValueEntity : AuditableEntity, IAggregateRoot
{
    public DefaultIdType DynamicPropertyId { get; set; }
    public string Value { get; set; } = null!;

    protected DynamicPropertyValueEntity()
    {
        // Only needed for working with dapper (See GetDynamicPropertyValueViaDapperRequest)
        // If you're not using dapper it's better to remove this constructor.
    }

    protected DynamicPropertyValueEntity(DefaultIdType dynamicPropertyId, string value)
    {
        DynamicPropertyId = dynamicPropertyId;
        Value = value;
    }
}