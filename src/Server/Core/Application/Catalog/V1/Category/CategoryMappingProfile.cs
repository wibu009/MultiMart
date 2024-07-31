using MultiMart.Application.Catalog.V1.Category.Create;
using MultiMart.Application.Catalog.V1.Category.Update;

namespace MultiMart.Application.Catalog.V1.Category;

public class CategoryMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCategoryRequest, Domain.Catalog.Category>()
            .IgnoreNullValues(true);
        config.NewConfig<UpdateCategoryRequest, Domain.Catalog.Category>()
            .Ignore(dest => dest.Id)
            .IgnoreNullValues(true);
    }
}