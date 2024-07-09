using Mapster;
using MultiMart.Application.Catalog.Product.Models;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Domain.Catalog;

namespace MultiMart.Infrastructure.Mapping.Catalog;

public class ProductMappingConfig : IRegister
{
    private readonly ISerializerService _serializerService;

    public ProductMappingConfig(ISerializerService serializerService) => _serializerService = serializerService;

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(dest => dest.BrandName, src => src.Brand.Name)
            .Map(dest => dest.BrandId, src => src.Brand.Id)
            .Map(dest => dest.DynamicProperties, src => src.DynamicProperties.Adapt<List<ProductDynamicPropertyDto>>());

        config.NewConfig<ProductDynamicPropertyValue, ProductDynamicPropertyDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.DynamicProperty.Name);
    }
}