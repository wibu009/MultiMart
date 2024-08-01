namespace MultiMart.Domain.Catalog.Addresses;

public class AddressOfSupplier : Address
{
    public DefaultIdType? SupplierId { get; set; }
    public Supplier Supplier { get; set; } = default!;
}