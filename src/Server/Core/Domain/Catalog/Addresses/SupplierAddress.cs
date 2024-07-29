namespace MultiMart.Domain.Catalog.Addresses;

public class SupplierAddress : Address
{
    public DefaultIdType? SupplierId { get; set; }
    public Supplier Supplier { get; set; } = default!;
}