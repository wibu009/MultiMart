using MultiMart.Domain.Common.Enums;

namespace MultiMart.Domain.Catalog.Addresses;

public class AddressOfUser : Address
{
    public AddressType Type { get; set; }
    public string? UserId { get; set; }
    public bool IsDefault { get; set; }
}