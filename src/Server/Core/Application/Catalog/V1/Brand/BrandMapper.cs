using MultiMart.Application.Catalog.V1.Brand.Create;
using MultiMart.Application.Catalog.V1.Brand.Update;

namespace MultiMart.Application.Catalog.V1.Brand;

public class BrandMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateBrandRequest, Domain.Catalog.Brand>()
            .Ignore(dest => dest.LogoUrl!)
            .IgnoreNullValues(true);
        config.NewConfig<UpdateBrandRequest, Domain.Catalog.Brand>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.LogoUrl!)
            .IgnoreNullValues(true);
    }
}