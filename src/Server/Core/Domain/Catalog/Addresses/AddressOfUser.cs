namespace MultiMart.Domain.Catalog.Addresses;

public class AddressOfUser : Address
{
    public string? UserId { get; set; }
    public bool IsDefault { get; set; }
}