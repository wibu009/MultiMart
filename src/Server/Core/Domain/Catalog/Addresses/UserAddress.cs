namespace MultiMart.Domain.Catalog.Addresses;

public class UserAddress : Address
{
    public string? UserId { get; set; }
    public bool IsDefault { get; set; }
}