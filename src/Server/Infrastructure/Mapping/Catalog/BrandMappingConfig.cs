using Mapster;
using MultiMart.Application.Catalog.Brand.Models;
using MultiMart.Application.Catalog.Brand.Requests;

namespace MultiMart.Infrastructure.Mapping.Catalog;

public class BrandMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Catalog.Brand, BrandDto>();
        config.NewConfig<CreateBrandRequest, Domain.Catalog.Brand>()
            .Ignore(dest => dest.LogoUrl ?? string.Empty);
        config.NewConfig<UpdateBrandRequest, Domain.Catalog.Brand>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.LogoUrl ?? string.Empty);
    }
}