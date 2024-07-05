using Mapster;
using MultiMart.Application.Catalog.Product.Dtos;
using MultiMart.Domain.Catalog;

namespace MultiMart.Infrastructure.Mapping.Catalog;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(dest => dest.BrandName, src => src.Brand.Name);
    }
}