using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Domain.Catalog;

public class Address : AuditableEntity, IAggregateRoot
{
    public string AddressLine1 { get; set; } = default!;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string Country { get; set; } = default!;
}

public class UserAddress : Address
{
    public UserAddressType Type { get; set; }
    public string? UserId { get; set; }
    public bool IsDefault { get; set; }
}
