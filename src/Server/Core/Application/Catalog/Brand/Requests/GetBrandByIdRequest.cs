using MultiMart.Application.Catalog.Brand.Models;

namespace MultiMart.Application.Catalog.Brand.Requests;

public class GetBrandByIdRequest : IRequest<BrandDto>
{
    public DefaultIdType Id { get; set; }
    public GetBrandByIdRequest(DefaultIdType id) => Id = id;
}