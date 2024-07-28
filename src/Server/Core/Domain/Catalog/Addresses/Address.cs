using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Domain.Catalog.Addresses;

public class Address : AuditableEntity, IAggregateRoot
{
    public string AddressLine1 { get; set; } = default!;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public AddressType Type { get; set; }
}

public class UserAddress : Address
{
    public string? UserId { get; set; }
    public bool IsDefault { get; set; }
}

public class SupplierAddress : Address
{
    public DefaultIdType? SupplierId { get; set; }
    public Supplier Supplier { get; set; } = default!;
}
