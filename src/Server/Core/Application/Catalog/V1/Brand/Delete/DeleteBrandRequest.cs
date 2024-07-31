namespace MultiMart.Application.Catalog.V1.Brand.Delete;

public class DeleteBrandRequest : IRequest<DeleteBrandResponse>
{
    public DefaultIdType Id { get; set; }
}