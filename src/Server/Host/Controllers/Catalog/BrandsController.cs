using BookStack.Application.Catalog.Brands;

namespace BookStack.Host.Controllers.Catalog;

[ApiVersion(VersionName.V1)]
public class BrandsController : VersionedApiController
{
    [HttpPost("search")]
    [AcceptLanguageHeader]
    [MustHavePermission(ApplicationAction.Search, ApplicationResource.Brands)]
    [OpenApiOperation("Search brands using available filters.", "")]
    public Task<PaginationResponse<BrandDto>> SearchAsync(SearchBrandsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [AcceptLanguageHeader]
    [MustHavePermission(ApplicationAction.View, ApplicationResource.Brands)]
    [OpenApiOperation("Get brand details.", "")]
    public Task<BrandDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetBrandRequest(id));
    }

    [HttpPost]
    [AcceptLanguageHeader]
    [MustHavePermission(ApplicationAction.Create, ApplicationResource.Brands)]
    [OpenApiOperation("Create a new brand.", "")]
    public Task<Guid> CreateAsync(CreateBrandRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [AcceptLanguageHeader]
    [MustHavePermission(ApplicationAction.Update, ApplicationResource.Brands)]
    [OpenApiOperation("Update a brand.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateBrandRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [AcceptLanguageHeader]
    [MustHavePermission(ApplicationAction.Delete, ApplicationResource.Brands)]
    [OpenApiOperation("Delete a brand.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteBrandRequest(id));
    }

    [HttpPost("generate-random")]
    [AcceptLanguageHeader]
    [MustHavePermission(ApplicationAction.Generate, ApplicationResource.Brands)]
    [OpenApiOperation("Generate a number of random brands.", "")]
    public Task<string> GenerateRandomAsync(GenerateRandomBrandRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpDelete("delete-random")]
    [AcceptLanguageHeader]
    [MustHavePermission(ApplicationAction.Clean, ApplicationResource.Brands)]
    [OpenApiOperation("Delete the brands generated with the generate-random call.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Search))]
    public Task<string> DeleteRandomAsync()
    {
        return Mediator.Send(new DeleteRandomBrandRequest());
    }
}