namespace MultiMart.Application.Catalog.Brand.Requests;

public class DeleteBrandRequest : IRequest<string>
{
    public DefaultIdType Id { get; set; }
    public DeleteBrandRequest(DefaultIdType id) => Id = id;
}