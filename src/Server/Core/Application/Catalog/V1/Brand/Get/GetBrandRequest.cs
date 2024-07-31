namespace MultiMart.Application.Catalog.V1.Brand.Get;

public class GetBrandRequest : IRequest<GetBrandResponse>
{
    public DefaultIdType Id { get; set; }
}