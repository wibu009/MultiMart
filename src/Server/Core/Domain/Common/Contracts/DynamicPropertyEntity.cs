using MultiMart.Domain.Catalog;

namespace MultiMart.Domain.Common.Contracts;

// The dynamic property entity will be used to store dynamic properties for entities.
public abstract class DynamicPropertyEntity : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsRequired { get; set; }

    protected DynamicPropertyEntity()
    {
        // Only needed for working with dapper (See GetDynamicPropertyViaDapperRequest)
        // If you're not using dapper it's better to remove this constructor.
    }

    protected DynamicPropertyEntity(string name, string type, string? description, bool isRequired)
    {
        Name = name;
        Type = type;
        Description = description;
        IsRequired = isRequired;
    }

    public Type GetPropertyType()
    {
        var type = System.Type.GetType(Type);
        if (type != null) return type;
        throw new InvalidOperationException();
    }
}